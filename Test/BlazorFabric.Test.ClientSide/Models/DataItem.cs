using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFabric.Test.ClientSide.Models
{
    public class DataItem
    {
        public DataItem(int num)
        {
            DisplayName = num.ToString();
        }

        public DataItem(string text)
        {
            DisplayName = text;
        }

        public DataItem(string text, SelectableOptionMenuItemType selectableOptionMenuItemType)
        {
            DisplayName = text;
            Type = selectableOptionMenuItemType;
        }
        public string DisplayName { get; set; }
        public string ImgUrl => "/background.png";

        public SelectableOptionMenuItemType Type { get; set; }
    }
}
