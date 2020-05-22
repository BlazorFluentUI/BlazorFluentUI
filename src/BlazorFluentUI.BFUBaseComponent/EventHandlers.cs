using Microsoft.AspNetCore.Components;
using System;

namespace BlazorFluentUI
{
    // FIX TO ADD THESE TWO EVENT HANDLERS THAT ARE MISSING FOR SOME REASON

    [EventHandler("onmouseenter", typeof(EventArgs))]
    [EventHandler("onmouseleave", typeof(EventArgs))]
    
    public static class EventHandlers
    {
    }
}
