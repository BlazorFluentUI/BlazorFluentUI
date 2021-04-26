namespace BlazorFluentUI
{
    public class NumberTextField<TValue> : TextFieldBase<TValue>
    {
        public NumberTextField()
        {
            InputType = InputType.Number;
            AutoComplete = AutoComplete.Off;
        }
    }
}
