using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorFabric
{
    public partial class Pivot : FabricComponentBase
    {
        [Parameter] public int DefaultSelectedIndex {get;set;}
        [Parameter] public string DefaultSelectedKey {get;set;}
        [Parameter] public bool HeadersOnly {get;set;}
        [Parameter] public PivotLinkFormat LinkFormat {get;set;}
        [Parameter] public PivotLinkSize LinkSize {get;set;}
        [Parameter] public EventCallback<MouseEventArgs> OnLinkClick {get;set;}
        [Parameter] public string SelectedKey{get;set;}
    }
}