using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public partial class BFUDocumentCardActivity : BFUComponentBase
    {
        public static Dictionary<string, string> GlobalClassNames = new Dictionary<string, string>()
        {
            {"root", "ms-DocumentCardActivity"},
            {"multiplePeople", "ms-DocumentCardActivity--multiplePeople"},
            {"details", "ms-DocumentCardActivity-details"},
            {"name", "ms-DocumentCardActivity-name"},
            {"activity", "ms-DocumentCardActivity-activity"},
            {"avatars", "ms-DocumentCardActivity-avatars"},
            {"avatar", "ms-DocumentCardActivity-avatar"}
        };
    }
}
