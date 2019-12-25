using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class RichTextEditor : FabricComponentBase
    {
        [Inject] private IJSRuntime jsRuntime { get; set; }

        [Parameter] public bool Disabled { get; set; }
               
        protected System.Collections.Generic.List<CommandBarItem> items;


        private RelayCommand buttonCommand;
        private int quillId;

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

            items = new System.Collections.Generic.List<CommandBarItem> {
                new CommandBarItem() { Text= "Bold", CanCheck=true, IconOnly=true, IconName="Bold", Key="Bold", Command=buttonCommand, CommandParameter="Bold"},
                new CommandBarItem() {Text= "Italic", CanCheck=true, IconOnly=true, IconName="Italic", Key="Italic", Command=buttonCommand, CommandParameter="Italic"},
                new CommandBarItem() {Text= "Underline", CanCheck=true, IconOnly=true, IconName="Underline", Key="Underline", Command=buttonCommand, CommandParameter="Underline"},
                new CommandBarItem() {Text= "Superscript", CanCheck=true, IconOnly=true, IconName="Superscript", Key="Superscript", Command=buttonCommand, CommandParameter="Superscript"},
                new CommandBarItem() {Text= "Subscript", CanCheck=true, IconOnly=true, IconName="Subscript", Key="Subscript", Command=buttonCommand, CommandParameter="Subscript"}
            };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                quillId = await jsRuntime.InvokeAsync<int>("window.BlazorFabricRichTextEditor.register", RootElementReference, DotNetObjectReference.Create(this));

            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected async Task UpdateFormatStateAsync()
        {
            var formatState = await jsRuntime.InvokeAsync<FormattingState>("window.BlazorFabricRichTextEditor.getFormat", quillId);

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
            //return Task.CompletedTask;
        }


    }
}
