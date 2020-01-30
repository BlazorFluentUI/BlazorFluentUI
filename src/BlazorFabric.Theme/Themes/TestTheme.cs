using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class TestTheme : ITheme
    {
        public IPrimary Primary => new TestPrimary();
        public IForeground Foreground => new TestForeground();
        public IBackground Background => new TestBackground();
    }
}
