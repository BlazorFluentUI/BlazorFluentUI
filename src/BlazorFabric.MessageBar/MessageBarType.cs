using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
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

    public static partial class CssClass
    {
        public static Dictionary<MessageBarType, string> MessageBarClass = new Dictionary<MessageBarType, string>
        {
            [MessageBarType.Info] = "",
            [MessageBarType.Warning] = "ms-MessageBar--warning",
            [MessageBarType.Error] = "ms-MessageBar--error",
            [MessageBarType.Blocked] = "ms-MessageBar--blocked",
            [MessageBarType.SevereWarning] = "ms-MessageBar--severeWarning",
            [MessageBarType.Success] = "ms-MessageBar--success"
        };
    }

}
