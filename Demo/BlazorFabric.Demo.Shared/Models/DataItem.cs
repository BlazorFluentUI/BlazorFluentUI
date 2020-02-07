using System;

namespace BlazorFabric.Demo.Shared.Models
{
    public class DataItem
    {
        public static Random random = new Random();

        public DataItem()
        {

        }

        public DataItem(int num)
        {
            Key = num.ToString();
            DisplayName = LoremUtils.Lorem(5); // = num.ToString();
            Description = LoremUtils.Lorem(10 + (int)Math.Round(random.NextDouble() * 50));
        }

        public DataItem(string text)
        {
            DisplayName = text;
            Key = text;
        }

        public DataItem(string text, SelectableOptionMenuItemType selectableOptionMenuItemType)
        {
            DisplayName = text;
            Type = selectableOptionMenuItemType;
        }
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public string ImgUrl => "redArrow.jpg";

        public SelectableOptionMenuItemType Type { get; set; }
    }
}
