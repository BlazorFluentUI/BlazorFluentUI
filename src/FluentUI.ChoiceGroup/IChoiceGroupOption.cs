namespace FluentUI
{
    public interface IChoiceGroupOption
    {
        string Label { get; }
        bool IsDisabled { get; }
        bool IsVisible { get; }
    }
}
