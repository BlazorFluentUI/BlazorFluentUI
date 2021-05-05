namespace BlazorFluentUI
{
    public class NumericTextField<TValue> : TextFieldBase<TValue>
    {
        public NumericTextField()
        {
            InputType = InputType.Number;
            AutoComplete = AutoComplete.Off;
        }
    }
}
