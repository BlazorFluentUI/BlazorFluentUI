using System.Collections.Generic;

namespace BlazorFluentUI
{
    public enum MessageBarType
    {
        Info,
        Warning,
        Error,
        Blocked,
        SevereWarning,
        Success
    }

    public static class MessageBarIcon
    {
        public static Dictionary<MessageBarType, string> IconMap = new()
        {
            [MessageBarType.Info] = "info",
            [MessageBarType.Warning] = "info",
            [MessageBarType.Error] = "error_circle",
            [MessageBarType.Blocked] = "block",
            [MessageBarType.SevereWarning] = "warning",
            [MessageBarType.Success] = "checkmark_circle"
        };
    }
}
