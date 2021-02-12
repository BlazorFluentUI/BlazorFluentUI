namespace FluentUI
{
    public interface ISelector
    {
        string SelectorName { get; set; }
        string GetSelectorAsString();
    }
}