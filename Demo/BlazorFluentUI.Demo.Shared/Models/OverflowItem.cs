namespace BlazorFluentUI.Demo.Shared.Models
{
    public class OverflowItem : IBFUOverflowSetItem
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public OverflowItem(int id)
        {
            Key = id.ToString();
            Name = id.ToString();
        }
    }
}
