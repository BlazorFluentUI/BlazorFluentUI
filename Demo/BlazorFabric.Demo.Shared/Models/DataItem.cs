namespace BlazorFabric.Demo.Shared.Models
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
        public string ImgUrl => "redArrow.jpg";

        public SelectableOptionMenuItemType Type { get; set; }
    }
}
