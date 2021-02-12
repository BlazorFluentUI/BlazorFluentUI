namespace FluentUI
{
    public class Rule : IRule
    {
        public ISelector Selector { get; set; }
        public IRuleProperties Properties { get; set; }
    }
}