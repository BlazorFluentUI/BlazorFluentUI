using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace BlazorFluentUI
{
    public class FocusTrapZoneProps
    {
        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; }

        [JsonPropertyName("disableFirstFocus")]
        public bool DisableFirstFocus { get; set; }

        [JsonPropertyName("elementToFocusOnDismiss")]
        public ElementReference ElementToFocusOnDismiss { get; set; }

        [JsonPropertyName("firstFocusableSelector")]
        public string? FirstFocusableSelector { get; set; }

        [JsonPropertyName("focusPreviouslyFocusedInnerElement")]
        public bool FocusPreviouslyFocusedInnerElement { get; set; }

        [JsonPropertyName("forceFocusInsideTrap")]
        public bool ForceFocusInsideTrap { get; set; }

        [JsonPropertyName("focusTriggerOnOutsideClick")]
        public bool FocusTriggerOnOutsideClick { get; set; }

        [JsonPropertyName("ignoreExternalFocusing")]
        public bool IgnoreExternalFocusing { get; set; }

        [JsonPropertyName("isClickableOutsideFocusTrap")]
        public bool IsClickableOutsideFocusTrap { get; set; }

        [JsonPropertyName("rootElement")]
        public ElementReference RootElement { get; set; }

        [JsonPropertyName("firstBumper")]
        public ElementReference FirstBumper { get; set; }

        [JsonPropertyName("lastBumper")]
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
            FocusTriggerOnOutsideClick = focusTrapZone.FocusTriggerOnOutsideClick;
            RootElement = focusTrapZone.RootElementReference;
            FirstBumper = firstBumper;
            LastBumper = lastBumper;
        }
    }
}
