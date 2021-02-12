using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public interface IFontWeight
    {
        public int Light { get; }
        public int SemiLight { get; }
        public int Regular { get; }
        public int SemiBold { get; }
        public int Bold { get; }
    }
}
