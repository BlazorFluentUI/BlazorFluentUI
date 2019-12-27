namespace BlazorFabric
{
    public class UniqueRule : IRule
    {
        public IUniqueSelector Selector { get; set; }
        public IRuleProperties Properties { get; set; }
    }
}