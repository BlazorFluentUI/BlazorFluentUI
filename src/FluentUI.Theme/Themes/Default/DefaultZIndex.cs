using FluentUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI.Themes.Default
{
    public class DefaultZIndex : IZIndex
    {
        public int FocusStyle => 1;

        public int Coachmark => 1000;

        public int Layer => 1000000;

        public int KeytipLayer => 1000001;

        public int Nav => 1;
    }
}
