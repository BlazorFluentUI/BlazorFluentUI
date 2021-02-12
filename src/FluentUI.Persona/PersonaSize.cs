using System.Linq;

namespace FluentUI
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

        public static int SizeToPixels(string size)
        {
            return int.Parse(size.Substring(0, size.Count() - 2));
        }
    }
}
