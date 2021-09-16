using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Timer = System.Timers.Timer;

namespace BlazorFluentUI
{
    public partial class RichTextEditor : FluentUIComponentBase, IDisposable
    {
        [Parameter] public bool Disabled { get; set; }

        [Parameter] public bool ReadOnly { get; set; }

        [Parameter] public string? RichText { get; set; }

        [Parameter] public EventCallback<string> RichTextChanged { get; set; }

        private List<CommandBarItem> items;
        //private bool hasFocus = false;

        private bool isImageDialogOpen = false;

        private string imageUrl = "";
        private string imageHeight = "";
        private string imageWidth = "";
        private string imageAlt = "";

        private string? internalRichText = "";  //keeps track of changes so we know when we have to update the quilljs contents.

        private RelayCommand buttonCommand;
        private DotNetObjectReference<RichTextEditor>? selfReference;
        private int quillId;
        private bool _renderedOnce;
        private Timer _debounceTextTimer;
        private string? _waitingText;
        private Timer _debounceSelectionTimer;
        private FormattingState? _waitingFormattingState;
        private bool _readonlySet;

        public RichTextEditor()
        {
            buttonCommand = new RelayCommand(async (p) =>
            {
                string s = p?.ToString()!;
                CommandBarItem? item = items?.FirstOrDefault(x => x.Key == s);
                if (item != null)
                {
                    if (item.CanCheck)
                    {
                        if (!item.Checked)
                            await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.setFormat", quillId, s.ToLowerInvariant());
                        else
                            await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.setFormat", quillId, s.ToLowerInvariant(), false);
                        item.Checked = !item.Checked;
                    }
                    else
                    {
                        switch (item.Key)
                        {
                            case "Image":
                                isImageDialogOpen = true;
                                //await jsRuntime.InvokeVoidAsync("window.BlazorFluentUIRichTextEditor.setFormat", quillId, "image", "";
                                break;
                        }

                    }
                }
                StateHasChanged();
            });

            _debounceTextTimer = new System.Timers.Timer
            {
                Interval = 150,
                AutoReset = false
            };
            _debounceTextTimer.Elapsed += async (s, e) =>
            {
                await InvokeAsync(async () =>
                {
                    await RichTextChanged.InvokeAsync(_waitingText);
                });
            };

            _debounceSelectionTimer = new System.Timers.Timer
            {
                Interval = 150,
                AutoReset = false
            };
            _debounceSelectionTimer.Elapsed += async (s, e) =>
            {
                await InvokeAsync(() =>
                {
                    if (_waitingFormattingState != null)
                    {
                        bool stateNeedsChanging = false;
                        PropertyInfo[]? props = _waitingFormattingState.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (PropertyInfo? prop in props)
                        {
                            CommandBarItem? commandButton = items?.FirstOrDefault(x => x.Key == prop.Name);
                            if (commandButton != null && commandButton.Checked != (bool?)prop.GetValue(_waitingFormattingState))
                            {
                                commandButton.Checked = !commandButton.Checked;
                                stateNeedsChanging = true;
                            }
                        }
                        if (stateNeedsChanging)
                            StateHasChanged();
                    }
                });
            };

            items = new System.Collections.Generic.List<CommandBarItem> {
                new CommandBarItem() { Text= "Bold", CanCheck=true, IconOnly=true, IconName="Bold", Key="Bold", Command=buttonCommand, CommandParameter="Bold"},
                new CommandBarItem() { Text= "Italic", CanCheck=true, IconOnly=true, IconName="Italic", Key="Italic", Command=buttonCommand, CommandParameter="Italic"},
                new CommandBarItem() { Text= "Underline", CanCheck=true, IconOnly=true, IconName="Underline", Key="Underline", Command=buttonCommand, CommandParameter="Underline"},
                new CommandBarItem() { Text= "Superscript", CanCheck=true, IconOnly=true, IconName="Superscript", Key="Superscript", Command=buttonCommand, CommandParameter="Superscript"},
                new CommandBarItem() { Text= "Subscript", CanCheck=true, IconOnly=true, IconName="Subscript", Key="Subscript", Command=buttonCommand, CommandParameter="Subscript"},

                new CommandBarItem() { Text= "Insert Image", CanCheck=false, IconOnly=true, IconName="ImagePixel", Key="Image", Command=buttonCommand, CommandParameter="Image"}
            };
        }

        [JSInvokable]
        public Task TextChangedAsync(TextChangedArgs args)
        {
            //if (args.Source != ChangeSource.User)
            {
                if (_debounceTextTimer.Enabled)
                    _debounceTextTimer.Stop();

                internalRichText = args.Html;

                if (args.Html != RichText)
                {
                    _waitingText = args.Html;
                    _debounceTextTimer.Start();
                }
            }
            return Task.CompletedTask;
        }

        [JSInvokable]
        public Task SelectionChangedAsync(FormattingState formattingState)
        {
            if (_debounceSelectionTimer.Enabled)
                _debounceSelectionTimer.Stop();

            _waitingFormattingState = formattingState;
            _debounceSelectionTimer.Start();

            return Task.CompletedTask;
        }

        protected override async Task OnParametersSetAsync()
        {
            if (_renderedOnce)
            {
                if (RichText != internalRichText)
                    await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.setHtmlContent", quillId, RichText!);
                if (ReadOnly && !_readonlySet)
                {
                    await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.setReadonly", quillId, true);
                    _readonlySet = true;
                }
                else if (!ReadOnly && _readonlySet)
                {
                    await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.setReadonly", quillId, false);
                    _readonlySet = false;
                }
            }
            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                selfReference = DotNetObjectReference.Create(this);
                quillId = await JSRuntime!.InvokeAsync<int>("BlazorFluentUIRichTextEditor.register", RootElementReference, selfReference);
                if (RichText != null)
                    await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.setHtmlContent", quillId, RichText);
                if (ReadOnly)
                {
                    await JSRuntime!.InvokeVoidAsync("window.BlazorFluentUIRichTextEditor.setReadonly", quillId, true);
                    _readonlySet = true;
                }
                _renderedOnce = true;

            }
            await base.OnAfterRenderAsync(firstRender);
        }


        private async Task UpdateFormatStateAsync()
        {
            FormattingState? formatState = await JSRuntime!.InvokeAsync<FormattingState>("BlazorFluentUIRichTextEditor.getFormat", quillId);
            if (formatState != null)
            {
                bool stateNeedsChanging = false;
                PropertyInfo[]? props = formatState.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo? prop in props)
                {
                    CommandBarItem? commandButton = items.FirstOrDefault(x => x.Key == prop.Name);
                    if (commandButton != null && commandButton.Checked != (bool?)prop.GetValue(formatState))
                    {
                        commandButton.Checked = !commandButton.Checked;
                        stateNeedsChanging = true;
                    }
                }
                if (stateNeedsChanging)
                    StateHasChanged();
            }

            //return Task.CompletedTask;
        }

        private async Task InterceptKeyPressAsync(KeyboardEventArgs keyboardEventArgs)
        {
            if (keyboardEventArgs.CtrlKey && keyboardEventArgs.Key == "+")
            {
                CommandBarItem? item = items.FirstOrDefault(x => x.Key == "Superscript");
                if (item != null)
                {
                    if (!item.Checked)
                        await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.setFormat", quillId, "superscript");
                    else
                        await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.setFormat", quillId, "superscript", false);


                    item.Checked = !item.Checked;
                }
            }
            else if (keyboardEventArgs.CtrlKey && keyboardEventArgs.Key == "=")
            {
                CommandBarItem? item = items.FirstOrDefault(x => x.Key == "Subscript");
                if (item != null)
                {
                    if (!item.Checked)
                        await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.setFormat", quillId, "subscript");
                    else
                        await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.setFormat", quillId, "subscript", false);


                    item.Checked = !item.Checked;
                }
            }
            //await UpdateFormatStateAsync();
        }

        private async Task OnFocusAsync()
        {
            await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.preventZoomEnable", true);
        }

        private async Task OnBlurAsync()
        {
            await JSRuntime!.InvokeVoidAsync("BlazorFluentUIRichTextEditor.preventZoomEnable", false);
        }

        private async Task InsertImageAsync()
        {
            await JSRuntime!.InvokeVoidAsync(
                "BlazorFluentUIRichTextEditor.insertImage",
                quillId,
                imageUrl,
                imageAlt,
                (string.IsNullOrWhiteSpace(imageWidth) ? null : imageWidth)!,
                (string.IsNullOrWhiteSpace(imageHeight) ? null : imageHeight)!);
            imageUrl = "";
            imageAlt = "";
            imageWidth = "";
            imageHeight = "";
            isImageDialogOpen = false;
        }

        public void Dispose()
        {
            selfReference?.Dispose();
        }
    }
}
