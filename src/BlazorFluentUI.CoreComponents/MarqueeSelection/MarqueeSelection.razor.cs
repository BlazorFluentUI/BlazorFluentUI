using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class MarqueeSelection<TItem> : FluentUIComponentBase, IAsyncDisposable
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public bool IsDraggingConstrainedToRoot { get; set; }
        [Parameter] public bool IsEnabled { get; set; } = true;
        [Parameter] public Func<bool>? OnShouldStartSelection { get; set; }
        [Parameter] public Selection<TItem>? Selection { get; set; }

        [Inject] private IJSRuntime? JSRuntime { get; set; }
        private const string scriptPath = "./_content/BlazorFluentUI.CoreComponents/marqueeSelection.js";
        private IJSObjectReference? scriptModule;


        [CascadingParameter] public SelectionZone<TItem>? SelectionZone { get; set; }


        private ManualRectangle? dragRect;
        private DotNetObjectReference<MarqueeSelection<TItem>>? selfReference;
        private MarqueeSelectionProps? props;

        public static Dictionary<string, string> GlobalClassNames = new()
        {
            {"root", "ms-MarqueeSelection"},
            {"dragMask", "ms-MarqueeSelection-dragMask"},
            {"box", "ms-MarqueeSelection-box"},
            {"boxFill", "ms-MarqueeSelection-boxFill"}
        };

        protected override async Task OnParametersSetAsync()
        {
            if (props == null)
            {
                props = GenerateProps();
            }

            if (selfReference != null)
            {
                if (IsEnabled != props!.IsEnabled
                    || IsDraggingConstrainedToRoot != props.IsDraggingConstrainedToRoot)
                {
                    props = GenerateProps();
                    await scriptModule!.InvokeVoidAsync("updateProps", selfReference, props);
                }
            }
            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (scriptModule == null)
                scriptModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", scriptPath);

            if (firstRender)
            {
                selfReference = DotNetObjectReference.Create(this);
                await scriptModule!.InvokeVoidAsync("registerMarqueeSelection", selfReference, RootElementReference, props);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private MarqueeSelectionProps GenerateProps()
        {
            return new MarqueeSelectionProps
            {
                IsDraggingConstrainedToRoot = IsDraggingConstrainedToRoot,
                IsEnabled = IsEnabled
            };
        }

        [JSInvokable]
        public Task<bool> OnShouldStartSelectionInternal()
        {
            if (OnShouldStartSelection == null)
                return Task.FromResult(true);
            else
                return Task.FromResult(OnShouldStartSelection.Invoke());
        }

        [JSInvokable]
        public void SetDragRect(ManualRectangle? manualRectangle)
        {
            //if (manualRectangle != null)
            //    Debug.WriteLine($"DragRect: {manualRectangle.top} {manualRectangle.left} {manualRectangle.height} {manualRectangle.width}");
            dragRect = manualRectangle;
            InvokeAsync(StateHasChanged);
        }

        [JSInvokable]
        public void UnselectAll()
        {
            Selection?.SetAllSelected(false);
        }

        [JSInvokable]
        public void SetChangeEvents(bool canProceed)
        {
            Selection?.SetChangeEvents(canProceed);
        }

        [JSInvokable]
        public void SetSelectedIndices(List<int> indices)
        {
            foreach (int index in indices)
            {
                Selection?.SetIndexSelected(index, true, false);
            }
            //Selection?.SetSelectedIndices(indices);
            //Debug.WriteLine($"Selected: {string.Join(',',indices)}");
            StateHasChanged();
        }

        [JSInvokable]
        public IEnumerable<int> GetSelectedIndicesAsync()
        {
            //return new List<int>(); //Selection?.SelectedIndices;
            return Selection!.GetSelectedIndices();
            //dragRect = manualRectangle;
        }

        public override async ValueTask DisposeAsync()
        {
            try
            {
                if (scriptModule != null)
                {
                    await scriptModule!.InvokeVoidAsync("unregisterMarqueeSelection", selfReference);
                    await scriptModule.DisposeAsync();
                }
                selfReference?.Dispose();

                await base.DisposeAsync();
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}
