using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorFabric
{
    public partial class RichTextEditor : FabricComponentBase
    {
        
        [Inject] private IJSRuntime jsRuntime { get; set; }

        [Parameter] public bool Disabled { get; set; }

        [Parameter] public string RichText { get; set; }

        [Parameter] public EventCallback<string> RichTextChanged { get; set; }
               
        protected System.Collections.Generic.List<CommandBarItem> items;
        protected bool hasFocus = false;

        private RelayCommand buttonCommand;
        private int quillId;
        private bool _renderedOnce;
        private Timer _debounceTextTimer;
        private string _waitingText;
        private Timer _debounceSelectionTimer;
        private FormattingState _waitingFormattingState;

        public RichTextEditor()
        {
            buttonCommand = new RelayCommand(async (p) =>
            {
                var item = items.FirstOrDefault(x => x.Key == p.ToString());
                if (item != null)
                {
                    if (!item.Checked)
                        await jsRuntime.InvokeVoidAsync("window.BlazorFabricRichTextEditor.setFormat", quillId, p.ToString().ToLowerInvariant());
                    else
                        await jsRuntime.InvokeVoidAsync("window.BlazorFabricRichTextEditor.setFormat", quillId, p.ToString().ToLowerInvariant(), false);


                    item.Checked = !item.Checked;
                }
                StateHasChanged();
            });

            _debounceTextTimer = new System.Timers.Timer();
            _debounceTextTimer.Interval = 150;
            _debounceTextTimer.AutoReset = false;
            _debounceTextTimer.Elapsed += async (s, e) => 
            {
                await InvokeAsync(async () =>
                {
                    await RichTextChanged.InvokeAsync(_waitingText);
                });
            };

            _debounceSelectionTimer = new System.Timers.Timer();
            _debounceSelectionTimer.Interval = 150;
            _debounceSelectionTimer.AutoReset = false;
            _debounceSelectionTimer.Elapsed += async (s, e) =>
            {
                await InvokeAsync(() =>
                {
                    if (_waitingFormattingState != null)
                    {
                        var stateNeedsChanging = false;
                        var props = _waitingFormattingState.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var prop in props)
                        {
                            var commandButton = items.FirstOrDefault(x => x.Key == prop.Name);
                            if (commandButton != null && commandButton.Checked != (bool)prop.GetValue(_waitingFormattingState))
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
                new CommandBarItem() {Text= "Italic", CanCheck=true, IconOnly=true, IconName="Italic", Key="Italic", Command=buttonCommand, CommandParameter="Italic"},
                new CommandBarItem() {Text= "Underline", CanCheck=true, IconOnly=true, IconName="Underline", Key="Underline", Command=buttonCommand, CommandParameter="Underline"},
                new CommandBarItem() {Text= "Superscript", CanCheck=true, IconOnly=true, IconName="Superscript", Key="Superscript", Command=buttonCommand, CommandParameter="Superscript"},
                new CommandBarItem() {Text= "Subscript", CanCheck=true, IconOnly=true, IconName="Subscript", Key="Subscript", Command=buttonCommand, CommandParameter="Subscript"}
            };
        }

        [JSInvokable]
        public Task TextChangedAsync(TextChangedArgs args)
        {
            //if (args.Source != ChangeSource.User)
            {
                if (_debounceTextTimer.Enabled)
                    _debounceTextTimer.Stop();

                if (args.Html != this.RichText)
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
                await jsRuntime.InvokeVoidAsync("window.BlazorFabricRichTextEditor.setHtmlContent", quillId, RichText);
            }
            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                quillId = await jsRuntime.InvokeAsync<int>("window.BlazorFabricRichTextEditor.register", RootElementReference, DotNetObjectReference.Create(this));
                await jsRuntime.InvokeVoidAsync("window.BlazorFabricRichTextEditor.setHtmlContent", quillId, RichText);
                _renderedOnce = true;

            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected async Task UpdateFormatStateAsync()
        {
            var formatState = await jsRuntime.InvokeAsync<FormattingState>("window.BlazorFabricRichTextEditor.getFormat", quillId);
            if (formatState != null)
            {
                var stateNeedsChanging = false;
                var props = formatState.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in props)
                {
                    var commandButton = items.FirstOrDefault(x => x.Key == prop.Name);
                    if (commandButton != null && commandButton.Checked != (bool)prop.GetValue(formatState))
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

        protected async Task InterceptKeyPressAsync(KeyboardEventArgs keyboardEventArgs)
        {
            if (keyboardEventArgs.CtrlKey && keyboardEventArgs.Key == "+")
            {
                var item = items.FirstOrDefault(x => x.Key == "Superscript");
                if (item != null)
                {
                    if (!item.Checked)
                        await jsRuntime.InvokeVoidAsync("window.BlazorFabricRichTextEditor.setFormat", quillId, "superscript");
                    else
                        await jsRuntime.InvokeVoidAsync("window.BlazorFabricRichTextEditor.setFormat", quillId, "superscript", false);


                    item.Checked = !item.Checked;
                }
            }
            else if (keyboardEventArgs.CtrlKey && keyboardEventArgs.Key == "=")
            {
                var item = items.FirstOrDefault(x => x.Key == "Subscript");
                if (item != null)
                {
                    if (!item.Checked)
                        await jsRuntime.InvokeVoidAsync("window.BlazorFabricRichTextEditor.setFormat", quillId, "subscript");
                    else
                        await jsRuntime.InvokeVoidAsync("window.BlazorFabricRichTextEditor.setFormat", quillId, "subscript", false);


                    item.Checked = !item.Checked;
                }
            }
            //await UpdateFormatStateAsync();
        }

        protected async Task OnFocusAsync()
        {
            await jsRuntime.InvokeVoidAsync("window.BlazorFabricRichTextEditor.preventZoomEnable", true);
        }

        protected async Task OnBlurAsync()
        {
            await jsRuntime.InvokeVoidAsync("window.BlazorFabricRichTextEditor.preventZoomEnable", false);
        }

    }
}
