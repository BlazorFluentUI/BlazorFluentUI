using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI.Models
{
    public class Keyframes
    {
        public static string DefaultKeyFrames = @"@keyframes Slide_down_in_10 {
    from {
        opacity: 0;
        transform: translate3d(0,-10px,0);
    }

    to {
        opacity: 1;
        transform: translate3d(0,0,0);
    }
}
@keyframes Slide_down_in_20 {
    from {
        opacity: 0;
        transform: translate3d(0,-20px,0);
    }
    to {
        opacity: 1;
        transform: translate3d(0,0,0);
    }
}
@keyframes Slide_up_in_10 {
    from {
        opacity: 0;
        transform: translate3d(0,10px,0);
    }

    to {
        opacity: 1;
        transform: translate3d(0,0,0);
    }
}
@keyframes Slide_right_in_10 {
    from {
        opacity: 0;
        transform: translate3d(-10px,0,0);
    }

    to {
        opacity: 1;
        transform: translate3d(0,0,0);
    }
}
@keyframes Slide_right_in_40{
    from {
        opacity: 0;
        transform: translate3d(-40px,0,0);
    }
    to {
        opacity: 1;
        transform: translate3d(0,0,0);
    }
}
@keyframes Slide_left_in_10 {
    from {
        opacity: 0;
        transform: translate3d(10px,0,0);
    }

    to {
        opacity: 1;
        transform: translate3d(0,0,0);
    }
}
@keyframes Slide_left_in_40 {
    from {
        opacity: 0;
        transform: translate3d(40px,0,0);
    }

    to {
        opacity: 1;
        transform: translate3d(0,0,0);
    }
}


@keyframes Slide_right_out_40 {
    from {
        opacity: 1;
        transform: translate3d(0p,0,0);
    }

    to {
        opacity: 0;
        transform: translate3d(40px,0,0);
    }
}
@keyframes Slide_left_out_40 {
    from {
        opacity: 1;
        transform: translate3d(0p,0,0);
    }

    to {
        opacity: 0;
        transform: translate3d(-40px,0,0);
    }
}

@keyframes Fade_in{
    from{
        opacity:0;
    }
    to{
        opacity:1;
    }
}
@keyframes Fade_out {
    from {
        opacity: 1;
    }

    to {
        opacity: 0;
    }
}";
    }
}
