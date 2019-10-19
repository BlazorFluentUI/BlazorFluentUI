using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    // FIX TO ADD THESE TWO EVENT HANDLERS THAT ARE MISSING FOR SOME REASON

    [EventHandler("onmouseenter", typeof(EventArgs))]
    [EventHandler("onmouseleave", typeof(EventArgs))]
    
    public static class EventHandlers
    {
    }
}
