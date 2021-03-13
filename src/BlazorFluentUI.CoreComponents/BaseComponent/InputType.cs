using System.Collections.Generic;

namespace BlazorFluentUI
{
    public enum InputType
    {
        Text,
        Password
    }

    public static class TypeUtils
    {
        public static Dictionary<InputType, string> InputMap = new()
        {
            [InputType.Text] = "text",
            [InputType.Password] = "password"
        };
    }
}
