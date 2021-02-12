using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FluentUI
{
    public class FocusZoneProps
    {
        [JsonPropertyName("allowFocusRoot")]
        public bool AllowFocusRoot { get; set; }

        [JsonPropertyName("checkForNoWrap")]
        public bool CheckForNoWrap { get; set; }

        [JsonPropertyName("defaultActiveElement")]
        public ElementReference DefaultActiveElement { get; set; }

        [JsonPropertyName("direction")]
        public FocusZoneDirection Direction { get; set; }

        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; }

        [JsonPropertyName("doNotAllowFocusEventToPropagate")]
        public bool DoNotAllowFocusEventToPropagate { get; set; }

        [JsonPropertyName("handleTabKey")]
        public FocusZoneTabbableElements HandleTabKey { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("innerZoneKeystrokeTriggers")]
        public List<ConsoleKey> InnerZoneKeystrokeTriggers { get; set; }

        [JsonPropertyName("isCircularNavigation")]
        public bool IsCircularNavigation { get; set; }

        [JsonPropertyName("onBeforeFocusExists")]
        public bool OnBeforeFocusExists { get; set; }

        [JsonPropertyName("root")]
        public ElementReference Root { get; set; }

        [JsonPropertyName("shouldInputLoseFocusOnArrowKeyExists")]
        public bool ShouldInputLoseFocusOnArrowKeyExists { get; set; }

        public static FocusZoneProps GenerateProps(FocusZone focusZone, string id, ElementReference root)
        {
            var props = new FocusZoneProps()
            {
                AllowFocusRoot = focusZone.AllowFocusRoot,
                CheckForNoWrap = focusZone.CheckForNoWrap,
                DefaultActiveElement = new ElementReference(focusZone.DefaultActiveElement),
                Direction = focusZone.Direction,
                Disabled=focusZone.Disabled,
                DoNotAllowFocusEventToPropagate=focusZone.DoNotAllowFocusEventToPropagate,
                HandleTabKey = focusZone.HandleTabKey,
                Id = id,
                InnerZoneKeystrokeTriggers = focusZone.InnerZoneKeystrokeTriggers,
                IsCircularNavigation =focusZone.IsCircularNavigation,
                OnBeforeFocusExists = focusZone.OnBeforeFocus != null,
                Root = root,
                ShouldInputLoseFocusOnArrowKeyExists = focusZone.ShouldInputLoseFocusOnArrowKey != null
            };

            return props;
        }
    }
}
