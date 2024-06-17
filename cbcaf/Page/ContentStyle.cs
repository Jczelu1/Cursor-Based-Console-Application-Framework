using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public class ContentStyle
    {
        public static string Cursor { get; set; } = ">";

        public static string StyleText(string text, Style style)
        {
            return $"\u001b[{(int)style}m{text}\u001b[0m";
        }
        public static string StyleText(string text, Style[] styles)
        {
            string formatCodes = string.Join(";", Array.ConvertAll(styles, style => ((int)style).ToString()));
            return $"\u001b[{formatCodes}m{text}\u001b[0m";
        }
    }
    public enum Style
    {
        // Text attributes
        Bold = 1,
        Dim = 2,
        Italic = 3,
        Underline = 4,
        BlinkSlow = 5,
        BlinkRapid = 6,
        Invert = 7,
        Hidden = 8,
        Strikethrough = 9,

        // Foreground colors
        FG_Black = 30,
        FG_Red = 31,
        FG_Green = 32,
        FG_Yellow = 33,
        FG_Blue = 34,
        FG_Magenta = 35,
        FG_Cyan = 36,
        FG_White = 37,
        FG_BrightBlack = 90,
        FG_BrightRed = 91,
        FG_BrightGreen = 92,
        FG_BrightYellow = 93,
        FG_BrightBlue = 94,
        FG_BrightMagenta = 95,
        FG_BrightCyan = 96,
        FG_BrightWhite = 97,

        // Background colors
        BG_Black = 40,
        BG_Red = 41,
        BG_Green = 42,
        BG_Yellow = 43,
        BG_Blue = 44,
        BG_Magenta = 45,
        BG_Cyan = 46,
        BG_White = 47,
        BG_BrightBlack = 100,
        BG_BrightRed = 101,
        BG_BrightGreen = 102,
        BG_BrightYellow = 103,
        BG_BrightBlue = 104,
        BG_BrightMagenta = 105,
        BG_BrightCyan = 106,
        BG_BrightWhite = 107
    }
}
