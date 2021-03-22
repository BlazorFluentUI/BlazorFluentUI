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
            [MessageBarType.Info] = "Info",
            [MessageBarType.Warning] = "Info",
            [MessageBarType.Error] = "ErrorBadge",
            [MessageBarType.Blocked] = "Blocked2",
            [MessageBarType.SevereWarning] = "Warning",
            [MessageBarType.Success] = "Completed"
        };
    }
}
