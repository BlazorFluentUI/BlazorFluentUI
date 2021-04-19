using System.Linq;

namespace BlazorFluentUI
{
    public static class PersonaSize
    {
        public const string Size8 = "20px";
        public const string Size24 = "24px";
        public const string Size32 = "32px";
        public const string Size40 = "40px";
        public const string Size48 = "48px";
        public const string Size56 = "56px";
        public const string Size72 = "72px";
        public const string Size100 = "100px";
        public const string Size120 = "120px";

        public static int SizeToPixels(string? size)
        {
            string? t = size?[0..^2];
            if (t != null)
                return int.Parse(t);

            return -1;
        }
    }
}
