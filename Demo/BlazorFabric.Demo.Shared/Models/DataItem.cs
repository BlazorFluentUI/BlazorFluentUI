namespace BlazorFabric.Demo.Shared.Models
{
    public class DataItem
    {
        public DataItem()
        {

        }

        public DataItem(int num)
        {
            Key = num.ToString();
            DisplayName = num.ToString();
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
        public string ImgUrl => "redArrow.jpg";

        public SelectableOptionMenuItemType Type { get; set; }
    }
}
