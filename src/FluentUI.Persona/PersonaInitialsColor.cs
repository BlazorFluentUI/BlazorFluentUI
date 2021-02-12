namespace FluentUI
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
        static PersonaInitialsColor[] _colorSwatchesLookup = new PersonaInitialsColor[]
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


        public static PersonaInitialsColor GetInitialsColorFromName(string displayName)
        {
            var color = PersonaInitialsColor.Blue;
            if (string.IsNullOrWhiteSpace(displayName))
                return color;

            var hashCode = 0;
            for (var iLen = displayName.Length - 1; iLen >= 0; iLen--)
            {
                var ch = displayName[iLen];
                var shift = iLen % 8;
                hashCode ^= (ch << shift) + (ch >> (8 - shift));
            }

            color = _colorSwatchesLookup[hashCode % _colorSwatchesLookup.Length];

            return color;
        }


        public static string GetPersonaColorHexCode(PersonaInitialsColor personaInitialsColor)
        {
            switch (personaInitialsColor)
            {
                case PersonaInitialsColor.LightBlue:
                    return "#4F6BED";
                case PersonaInitialsColor.Blue:
                    return "#0078D4";
                case PersonaInitialsColor.DarkBlue:
                    return "#004E8C";
                case PersonaInitialsColor.Teal:
                    return "#038387";
                case PersonaInitialsColor.LightGreen:
                case PersonaInitialsColor.Green:
                    return "#498205";
                case PersonaInitialsColor.DarkGreen:
                    return "#0B6A0B";
                case PersonaInitialsColor.LightPink:
                    return "#C239B3";
                case PersonaInitialsColor.Pink:
                    return "#E3008C";
                case PersonaInitialsColor.Magenta:
                    return "#881798";
                case PersonaInitialsColor.Purple:
                    return "#5C2E91";
                case PersonaInitialsColor.Orange:
                    return "#CA5010";
                case PersonaInitialsColor.Red:
                    return "#EE1111";
                case PersonaInitialsColor.LightRed:
                    return "#D13438";
                case PersonaInitialsColor.DarkRed:
                    return "#A4262C";
                case PersonaInitialsColor.Transparent:
                    return "transparent";
                case PersonaInitialsColor.Violet:
                    return "#8764B8";
                case PersonaInitialsColor.Gold:
                    return "#986F0B";
                case PersonaInitialsColor.Burgundy:
                    return "#750B1C";
                case PersonaInitialsColor.WarmGray:
                    return "#7A7574";
                case PersonaInitialsColor.Cyan:
                    return "#005B70";
                case PersonaInitialsColor.Rust:
                    return "#8E562E";
                case PersonaInitialsColor.CoolGray:
                    return "#69797E";
                case PersonaInitialsColor.Black:
                    return "#1D1D1D";
                case PersonaInitialsColor.Gray:
                    return "#393939";
                default:
                    return "#0078D4";
            }
        }
    }



}
