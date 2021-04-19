namespace BlazorFluentUI
{
    public enum PersonaInitialsColor
    {
        LightBlue,
        Blue,
        DarkBlue,
        Teal,
        LightGreen,
        Green,
        DarkGreen,
        LightPink,
        Pink,
        Magenta,
        Purple,
        Black, //special
        Orange,
        Red, //special
        DarkRed,
        Transparent, //special
        Violet,
        LightRed,
        Gold,
        Burgundy,
        WarmGray,
        CoolGray,
        Gray, //special
        Cyan,
        Rust,

    }

    internal static class PersonaColorUtils
    {
        static readonly PersonaInitialsColor[] _colorSwatchesLookup = new PersonaInitialsColor[]
        {
             PersonaInitialsColor.LightBlue,
        PersonaInitialsColor.Blue,
        PersonaInitialsColor.DarkBlue,
        PersonaInitialsColor.Teal,
        PersonaInitialsColor.Green,
        PersonaInitialsColor.DarkGreen,
        PersonaInitialsColor.LightPink,
        PersonaInitialsColor.Pink,
        PersonaInitialsColor.Magenta,
        PersonaInitialsColor.Purple,
        PersonaInitialsColor.Orange,
        PersonaInitialsColor.LightRed,
        PersonaInitialsColor.DarkRed,
        PersonaInitialsColor.Violet,
        PersonaInitialsColor.Gold,
        PersonaInitialsColor.Burgundy,
        PersonaInitialsColor.WarmGray,
        PersonaInitialsColor.Cyan,
        PersonaInitialsColor.Rust,
        PersonaInitialsColor.CoolGray
        };


        public static PersonaInitialsColor GetInitialsColorFromName(string? displayName)
        {
            PersonaInitialsColor color = PersonaInitialsColor.Blue;
            if (string.IsNullOrWhiteSpace(displayName))
                return color;

            int hashCode = 0;
            for (int iLen = displayName.Length - 1; iLen >= 0; iLen--)
            {
                char ch = displayName[iLen];
                int shift = iLen % 8;
                hashCode ^= (ch << shift) + (ch >> (8 - shift));
            }

            color = _colorSwatchesLookup[hashCode % _colorSwatchesLookup.Length];

            return color;
        }


        public static string GetPersonaColorHexCode(PersonaInitialsColor personaInitialsColor)
        {
            return personaInitialsColor switch
            {
                PersonaInitialsColor.LightBlue => "#4F6BED",
                PersonaInitialsColor.Blue => "#0078D4",
                PersonaInitialsColor.DarkBlue => "#004E8C",
                PersonaInitialsColor.Teal => "#038387",
                PersonaInitialsColor.LightGreen or PersonaInitialsColor.Green => "#498205",
                PersonaInitialsColor.DarkGreen => "#0B6A0B",
                PersonaInitialsColor.LightPink => "#C239B3",
                PersonaInitialsColor.Pink => "#E3008C",
                PersonaInitialsColor.Magenta => "#881798",
                PersonaInitialsColor.Purple => "#5C2E91",
                PersonaInitialsColor.Orange => "#CA5010",
                PersonaInitialsColor.Red => "#EE1111",
                PersonaInitialsColor.LightRed => "#D13438",
                PersonaInitialsColor.DarkRed => "#A4262C",
                PersonaInitialsColor.Transparent => "transparent",
                PersonaInitialsColor.Violet => "#8764B8",
                PersonaInitialsColor.Gold => "#986F0B",
                PersonaInitialsColor.Burgundy => "#750B1C",
                PersonaInitialsColor.WarmGray => "#7A7574",
                PersonaInitialsColor.Cyan => "#005B70",
                PersonaInitialsColor.Rust => "#8E562E",
                PersonaInitialsColor.CoolGray => "#69797E",
                PersonaInitialsColor.Black => "#1D1D1D",
                PersonaInitialsColor.Gray => "#393939",
                _ => "#0078D4",
            };
        }
    }



}
