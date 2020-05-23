namespace BlazorFluentUI.BFUChoiceGroup
{
    public interface IChoiceGroupOption
    {
        string Text { get; }
        bool IsDisabled { get; }
        bool IsVisible { get; }
    }
}
