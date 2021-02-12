using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FluentUI
{
    public class FormattingState
    {
        [JsonPropertyName("bold")]
        public bool Bold { get; set; }

        [JsonPropertyName("italic")]
        public bool Italic { get; set; }

        [JsonPropertyName("underline")]
        public bool Underline { get; set; }

        [JsonPropertyName("superscript")]
        public bool Superscript { get; set; }

        [JsonPropertyName("subscript")]
        public bool Subscript { get; set; }

    }
}
