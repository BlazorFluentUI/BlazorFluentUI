using Microsoft.AspNetCore.Components;

namespace FluentUI.FocusTrapZoneInternal
{
    public class FocusTrapZoneProps
    {      
        public bool Disabled { get; set; }
        public bool DisableFirstFocus { get; set; }
        public ElementReference ElementToFocusOnDismiss { get; set; }
        public string FirstFocusableSelector { get; set; }
        public bool FocusPreviouslyFocusedInnerElement { get; set; }
        public bool ForceFocusInsideTrap { get; set; }
        public bool IgnoreExternalFocusing { get; set; }
        public bool IsClickableOutsideFocusTrap { get; set; }

        public ElementReference RootElement { get; set; }
        public ElementReference FirstBumper { get; set; }
        public ElementReference LastBumper { get; set; }

        public FocusTrapZoneProps(FocusTrapZone focusTrapZone, ElementReference firstBumper, ElementReference lastBumper)
        {
            Disabled = focusTrapZone.Disabled;
            DisableFirstFocus = focusTrapZone.DisableFirstFocus;
            ElementToFocusOnDismiss = focusTrapZone.ElementToFocusOnDismiss;
            FirstFocusableSelector = focusTrapZone.FirstFocusableSelector;
            FocusPreviouslyFocusedInnerElement = focusTrapZone.FocusPreviouslyFocusedInnerElement;
            ForceFocusInsideTrap = focusTrapZone.ForceFocusInsideTrap;
            IgnoreExternalFocusing = focusTrapZone.IgnoreExternalFocusing;
            IsClickableOutsideFocusTrap = focusTrapZone.IsClickableOutsideFocusTrap;
            RootElement = focusTrapZone.RootElementReference;
            FirstBumper = firstBumper;
            LastBumper = lastBumper;
        }
    }
}
