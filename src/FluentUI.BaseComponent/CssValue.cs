namespace FluentUI
{
    public class CssValue
    {
        public CssValue(object initialValue)
        {
            Value = initialValue;
        }

        public object Value { get; set; }

        public static implicit operator CssValue(in double value) => new CssValue(value);
        public static implicit operator CssValue(in bool value) => new CssValue(value);
        public static implicit operator CssValue(in string value) => new CssValue(value);
        public static implicit operator CssValue(in Css value) => new CssValue(value);

        public string AsLength => (Value is string ? (string)Value : (Value is double ? (double)Value + "px" : ""));
        public bool AsBooleanTrueExplicit => (Value is bool ? (bool)Value : false);
        public string AsString => (Value is string ? (string)Value : (Value is Css ? ((Css)Value).ToString() : (Value is double ? ((double)Value).ToString() : "0")));
    }

    public enum Css
    {
        Inherit,
        Initial,
        Unset
    }

    //public class CssValue<T> : CssValue
    //{
        
    //    public CssValue(T initialValue)
    //    {
    //        Value = initialValue;
    //    }

    //    public T Value { get; set; }

    //    public override object ObjectValue => Value;
    //}
}
