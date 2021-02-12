using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI.Style
{
    public static class ContentStyle
    {
        public static string HiddenContentStyle()
        {
            return $"position:absolute;" +
                    $"width:1px;" +
                    $"height:1px;" +
                    $"margin:-1px;" +
                    $"padding:0px;" +
                    $"overflow:hidden;";
        }
    }
}
