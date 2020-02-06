namespace BlazorFabric
{
    public class LocalRule : IRule
    {
        public IUniqueSelector Selector { get; set; }
        public IRuleProperties Properties { get; set; }
    }
}