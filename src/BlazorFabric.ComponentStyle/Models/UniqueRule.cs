namespace BlazorFabric
{
    public class DynamicRule : IRule
    {
        public IUniqueSelector Selector { get; set; }
        public IRuleProperties Properties { get; set; }
    }
}