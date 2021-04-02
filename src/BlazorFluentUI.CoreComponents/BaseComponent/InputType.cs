using System.Collections.Generic;

namespace BlazorFluentUI
{
    public enum InputType
    {
        Text,
        Password,
        Number,         //min,max,step,value
        //Email,
        //Range,          //min,max,step,value
        //Search,
        //Tel,            //pattern
        //Url,

        //Date,
        //DateTimeLocal,
        //Time,
        //Week,
        //Month,

        //Checkbox,
        //Radio,
        //Color,

        //File,
        //Button,
        //Image,
        //Hidden,
        //Reset,
        //Submit
    }

    public static class TypeUtils
    {
        public static Dictionary<InputType, string> InputMap = new()
        {
            [InputType.Text] = "text",
            [InputType.Password] = "password",
            [InputType.Number] = "number",
        };
    }
}
