using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI.Interfaces
{
    public interface IZIndex
    {
        public int FocusStyle { get; }
        public int Coachmark { get; }
        public int Layer { get; }
        public int KeytipLayer { get; }
        public int Nav { get; }
    }
}