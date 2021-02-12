using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public interface IAnimation
    {
        string Duration1 { get; }
        string Duration2 { get; }
        string Duration3 { get; }
        string Duration4 { get; }

        string EasingFunction1 { get; }
        string EasingFunction2 { get; }


        //string SlideDownIn10 { get; }
        //string SlideDownIn20 { get; }
        //string SlideRightIn10 { get; }
        //string SlideRightIn40 { get; }
        //string SlideUpIn10 { get; }
        //string SlideLeftIn10 { get; }
        //string SlideLeftIn40 { get; }

        //string SlideRightOut40 { get; }
        //string SlideLeftOut40 { get; }

        //string FadeIn100 { get; }
        //string FadeOut100 { get; }
        //string FadeIn200 { get; }
        //string FadeOut200 { get; }
        //string FadeIn400 { get; }
    }
}
