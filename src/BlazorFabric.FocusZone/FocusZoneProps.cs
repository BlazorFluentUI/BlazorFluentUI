using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class FocusZoneProps
    {
        [System.Text.Json.Serialization.JsonPropertyName("allowFocusRoot")]
        public bool AllowFocusRoot { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("checkForNoWrap")]
        public bool CheckForNoWrap { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("defaultActiveElement")]
        public ElementReference DefaultActiveElement { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("direction")]
        public FocusZoneDirection Direction { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("disabled")]
        public bool Disabled { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("doNotAllowFocusEventToPropagate")]
        public bool DoNotAllowFocusEventToPropagate { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("handleTabKey")]
        public FocusZoneTabbableElements HandleTabKey { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public string Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("innerZoneKeystrokeTriggers")]
        public List<ConsoleKey> InnerZoneKeystrokeTriggers { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("isCircularNavigation")]
        public bool IsCircularNavigation { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("onBeforeFocusExists")]
        public bool OnBeforeFocusExists { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("root")]
        public ElementReference Root { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("shouldInputLoseFocusOnArrowKeyExists")]
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
