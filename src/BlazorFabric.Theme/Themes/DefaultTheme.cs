namespace BlazorFabric
{
    public class DefaultTheme : ITheme
    {
        public IPrimary Primary => new DefaultPrimary();
        public IForeground Foreground => new DefaultForeground();
        public IBackground Background => new DefaultBackground();
    }
}
