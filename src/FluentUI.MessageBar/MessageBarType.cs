using System.Collections.Generic;

namespace FluentUI
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
        public static Dictionary<MessageBarType, string> IconMap = new Dictionary<MessageBarType, string>
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
