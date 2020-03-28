using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class FabricComponentBase : ComponentBase
    {
        [CascadingParameter(Name = "Theme")]
        public ITheme Theme { get; set; }

        //[Inject] private IComponentContext ComponentContext { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private ThemeProvider ThemeProvider { get; set; }

        [Parameter] public string ClassName { get; set; }
        [Parameter] public string Style { get; set; }

        //ARIA Properties
        [Parameter] public string AriaAtomic { get; set; }
        [Parameter] public string AriaBusy { get; set; }
        [Parameter] public string AriaControls { get; set; }
        [Parameter] public string AriaCurrent { get; set; }
        [Parameter] public string AriaDescribedBy { get; set; }
        [Parameter] public string AriaDetails { get; set; }
        [Parameter] public bool AriaDisabled { get; set; }
        [Parameter] public string AriaDropEffect { get; set; }
        [Parameter] public string AriaErrorMessage { get; set; }
        [Parameter] public string AriaFlowTo { get; set; }
        [Parameter] public string AriaGrabbed { get; set; }
        [Parameter] public string AriaHasPopup { get; set; }
        [Parameter] public string AriaHidden { get; set; }
        [Parameter] public string AriaInvalid { get; set; }
        [Parameter] public string AriaKeyShortcuts { get; set; }
        [Parameter] public string AriaLabel { get; set; }
        [Parameter] public string AriaLabelledBy { get; set; }
        [Parameter] public AriaLive AriaLive { get; set; } = AriaLive.Polite;
        [Parameter] public string AriaOwns { get; set; }
        [Parameter] public bool AriaReadonly { get; set; }  //not universal
        [Parameter] public string AriaRelevant { get; set; }
        [Parameter] public string AriaRoleDescription { get; set; }

        public ElementReference RootElementReference;

        private ITheme _theme;
        private bool reloadStyle;

        [Inject] ScopedStatics ScopedStatics { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<GlobalCS>(0);
            builder.AddAttribute(1, "Component", new FabricComponentBase());
            builder.AddAttribute(2, "CreateGlobalCss", new System.Func<ICollection<Rule>>( CreateGlobalCss ));
            builder.AddAttribute(3, "ReloadStyle", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<bool>(reloadStyle));
            builder.AddAttribute(4, "ReloadStyleChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(EventCallback.Factory.Create(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => reloadStyle = __value, reloadStyle))));
            builder.AddAttribute(5, "FixStyle", true);

            builder.CloseComponent();
            base.BuildRenderTree(builder);
        }

        protected override void OnInitialized()
        {
            ThemeProvider.ThemeChanged += OnThemeChangedPrivate;
            ThemeProvider.ThemeChanged += OnThemeChangedProtected;
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!ScopedStatics.FocusRectsInitialized)
            {
                ScopedStatics.FocusRectsInitialized = true;
                await JSRuntime.InvokeVoidAsync("BlazorFabricBaseComponent.initializeFocusRects");
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task<Rectangle> GetBoundsAsync()
        {
            try
            {
                var rectangle = await JSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.measureElementRect", RootElementReference);
                return rectangle;
            }
            catch (JSException ex)
            {
                return new Rectangle();
            }
        }

        public async Task<Rectangle> GetBoundsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var rectangle = await JSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.measureElementRect", cancellationToken, RootElementReference);
                return rectangle;
            }
            catch (JSException ex)
            {
                return new Rectangle();
            }
        }

        public async Task<Rectangle> GetBoundsAsync(ElementReference elementReference, CancellationToken cancellationToken)
        {
            try
            {
                var rectangle = await JSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.measureElementRect", cancellationToken, elementReference);
                return rectangle;
            }
            catch (JSException ex)
            {
                return new Rectangle();
            }
        }

        public async Task<Rectangle> GetBoundsAsync(ElementReference elementReference)
        {
            try
            {
                var rectangle = await JSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.measureElementRect", elementReference);
                return rectangle;
            }
            catch (JSException ex)
            {
                return new Rectangle();
            }
        }
        
        private void OnThemeChangedProtected(object sender, ThemeChangedArgs themeChangedArgs)
        {
            Theme = themeChangedArgs.Theme;
            OnThemeChanged();
        }

        protected virtual void OnThemeChanged() { }

        private void OnThemeChangedPrivate(object sender, ThemeChangedArgs themeChangedArgs)
        {
            reloadStyle = true;
        }

        private ICollection<Rule> CreateGlobalCss()
        {
            var overallRules = new HashSet<Rule>();
            overallRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "body" },
                Properties = new CssString()
                {
                    Css = $"-moz-osx-font-smoothing:grayscale;" +
                            $"-webkit-font-smoothing:antialiased;" +
                            $"color:{Theme?.SemanticTextColors?.BodyText ?? "#323130"};" +
                            $"background-color:{Theme?.SemanticColors?.BodyBackground ?? "#ffffff"};" +
                            $"font-family:'Segoe UI Web (West European)', 'Segoe UI', -apple-system, BlinkMacSystemFont, 'Roboto', 'Helvetica Neue', sans-serif;" +
                            $"font-size:14px;"
                }
            });
            return overallRules;
        }
    }
}
