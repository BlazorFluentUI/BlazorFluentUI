using System.Collections.Generic;

namespace BlazorFabric
{
    public enum InputType
    {
        Text,
        Password
    }

    public static class TypeUtils
    {
        public static Dictionary<InputType, string> InputMap = new Dictionary<InputType, string>
        {
            [InputType.Text] = "text",
            [InputType.Password] = "password"
        };
    }
}
