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
    public partial class BFUMarqueeSelection<TItem> : BFUComponentBase, IAsyncDisposable
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public bool IsDraggingConstrainedToRoot { get; set; }
        [Parameter] public bool IsEnabled { get; set; }
        [Parameter] public Func<bool>? OnShouldStartSelection { get; set; }
        [Parameter] public Selection<TItem>? Selection { get; set; }

        [Inject] private IJSRuntime? JSRuntime { get; set; }

        [CascadingParameter] public BFUSelectionZone<TItem>? SelectionZone { get; set; }


        private ManualRectangle? dragRect;
        private DotNetObjectReference<BFUMarqueeSelection<TItem>>? dotNetRef;
        private BFUMarqueeSelectionProps props;

        public static Dictionary<string, string> GlobalClassNames = new Dictionary<string, string>()
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

            if (dotNetRef != null)
            {
                if (IsEnabled != props!.IsEnabled
                    || IsDraggingConstrainedToRoot != props.IsDraggingConstrainedToRoot)
                {
                    props = GenerateProps();
                    await JSRuntime!.InvokeVoidAsync("BlazorFluentUiMarqueeSelection.updateProps", dotNetRef, props);
                }
            }
            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dotNetRef = DotNetObjectReference.Create(this);
                await JSRuntime!.InvokeVoidAsync("BlazorFluentUiMarqueeSelection.registerMarqueeSelection", dotNetRef, RootElementReference, props);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private BFUMarqueeSelectionProps GenerateProps()
        {
            return new BFUMarqueeSelectionProps
            {
                IsDraggingConstrainedToRoot = this.IsDraggingConstrainedToRoot,
                IsEnabled = this.IsEnabled
            };
        }

        //public ICollection<IRule> CreateGlobalCss(ITheme theme)
        //{
        //    var marqueeRules = new HashSet<IRule>();
        //    marqueeRules.AddCssStringSelector($".{GlobalClassNames["root"]}")
        //        .AppendCssStyles(
        //        "position:relative",
        //        "cursor:default"
        //        );

        //    marqueeRules.AddCssStringSelector($".{GlobalClassNames["dragMask"]}")
        //        .AppendCssStyles(
        //        "position:absolute",
        //        "background:rgba(255,0,0,0)",
        //        "left:0",
        //        "top:0",
        //        "right:0",
        //        "bottom:0"
        //        );

        //    marqueeRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
        //        Properties = new CssString()
        //        {
        //            Css = $".{GlobalClassNames["dragMask"]} {{ background:none; background-color:transparent;  }}"
        //        }
        //    });

        //    marqueeRules.AddCssStringSelector($".{GlobalClassNames["box"]}")
        //      .AppendCssStyles(
        //      "position:absolute",
        //      "box-sizing:border-box",
        //      "border:1px solid var(--palette-ThemePrimary)",
        //      "z-index:10"
        //      );


        //    marqueeRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
        //        Properties = new CssString()
        //        {
        //            Css = $".{GlobalClassNames["box"]} {{ border-color:Highlight; }}"
        //        }
        //    });

        //    marqueeRules.AddCssStringSelector($".{GlobalClassNames["boxFill"]}")
        //     .AppendCssStyles(
        //     "position:absolute",
        //     "box-sizing:border-box",
        //     "background-color: var(--palette-ThemePrimary)",
        //     "opacity:0.1",
        //     "left:0",
        //     "top:0",
        //     "right:0",
        //     "bottom:0"
        //     );

        //    marqueeRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
        //        Properties = new CssString()
        //        {
        //            Css = $".{GlobalClassNames["boxFill"]} {{ background:none; background-color:transparent; }}"
        //        }
        //    });


        //    return marqueeRules;
        //}

        

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
            foreach (var index in indices)
            {
                Selection.SetIndexSelected(index, true, false);
            }
            //Selection?.SetSelectedIndices(indices);
            //Debug.WriteLine($"Selected: {string.Join(',',indices)}");
            StateHasChanged();
        }

        [JSInvokable]
        public IEnumerable<int> GetSelectedIndicesAsync()
        {
            //return new List<int>(); //Selection?.SelectedIndices;
            return Selection?.GetSelectedIndices();
            //dragRect = manualRectangle;
        }

        public async ValueTask DisposeAsync()
        {
            if (dotNetRef != null)
            {
                await JSRuntime!.InvokeVoidAsync("BlazorFluentUiMarqueeSelection.unregisterMarqueeSelection", dotNetRef);
                dotNetRef?.Dispose();
            }
        }
    }
}
