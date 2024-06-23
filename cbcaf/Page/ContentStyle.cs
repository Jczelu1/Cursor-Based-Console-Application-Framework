using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    /*
    public static class ContentStyle
    {
        private static readonly Regex AnsiEscapeRegex = new Regex(@"\x1B\[[0-9;]*[mGKHF]", RegexOptions.Compiled);

        public static string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // Remove ANSI escape sequences
            return AnsiEscapeRegex.Replace(text, string.Empty);
        }
        public static string GetSubstringWithoutAnsi(string text, int length)
        {
            if (string.IsNullOrEmpty(text) || length <= 0)
                return string.Empty;

            // Remove ANSI escape sequences
            string cleanedText = CleanText(text);

            // Get the substring of the desired length from the cleaned text
            string substring = cleanedText.Length > length ? cleanedText.Substring(0, length) : cleanedText;

            // Reinsert ANSI escape sequences into the substring
            return ReinsertAnsiSequences(text, substring) + "\u001b[0m\u001b[999m";
        }
        public static List<string> WrapText(string text, int length)
        {
            if (string.IsNullOrEmpty(text) || length <= 0)
                return new List<string> { string.Empty };

            string cleanedText = CleanText(text);
            List<string> lines = new List<string>();
            int start = 0;

            while (start < cleanedText.Length)
            {
                int end = Math.Min(start + length, cleanedText.Length);

                // Find the last newline within the current segment to break the line
                int newlineIndex = cleanedText.IndexOf('\n', start, end - start);
                if (newlineIndex != -1)
                {
                    end = newlineIndex;
                }
                else
                {
                    // Find the last space within the current segment to break the line
                    if (end < cleanedText.Length && cleanedText[end] != ' ')
                    {
                        int lastSpace = cleanedText.LastIndexOf(' ', end - 1, end - start);
                        if (lastSpace > start)
                        {
                            end = lastSpace;
                        }
                    }
                }

                string segment = cleanedText.Substring(start, end - start).TrimEnd();
                string formattedSegment = ReinsertAnsiSequences(text, segment);
                lines.Add(formattedSegment);

                start = end;

                // Skip newline or spaces at the beginning of the next line
                while (start < cleanedText.Length && (cleanedText[start] == ' ' || cleanedText[start] == '\n'))
                {
                    start++;
                }
            }

            return lines;
        }


        private static string ReinsertAnsiSequences(string original, string cleanedSubstring)
        {
            StringBuilder result = new StringBuilder();
            int cleanedIndex = 0;
            bool inEscapeSequence = false;

            foreach (char c in original)
            {
                if (c == '\x1B')
                {
                    inEscapeSequence = true;
                    result.Append(c);
                }
                else if (inEscapeSequence)
                {
                    result.Append(c);
                    if (c == 'm' || c == 'G' || c == 'K' || c == 'H' || c == 'F')
                    {
                        inEscapeSequence = false;
                    }
                }
                else
                {
                    if (cleanedIndex < cleanedSubstring.Length && cleanedSubstring[cleanedIndex] == c)
                    {
                        result.Append(c);
                        cleanedIndex++;
                    }
                    else if (cleanedIndex >= cleanedSubstring.Length)
                    {
                        break;
                    }
                }
            }

            return result.ToString();
        }

    public static string Cursor { private get; set; } = ">";

        public static List<Style> CursorStyles = new List<Style>();

        internal static List<Style> BaseStyles = new List<Style>();

        public static string GetCursor()
        {
            return StyleContentText(Cursor, CursorStyles);
        }

        public static string GetStyleString(Style style)
        {
            return $"\u001b[{(int)style}m";
        }

        public static string GetStyleString(List<Style> styles)
        {
            string formatCodes = string.Join(";", styles.ConvertAll(style => ((int)style).ToString()));
            return $"\u001b[{formatCodes}m";
        }
        internal static void ApplyBaseStyle(Style style)
        {
            Console.Write(GetStyleString(style));
            BaseStyles = new List<Style>() { style };
        }
        internal static void ApplyBaseStyles(List<Style> styles)
        {
            Console.Write(GetStyleString(styles));
            BaseStyles = styles;
        }
        internal static void ResetToBaseStyles()
        {
            ApplyBaseStyles(BaseStyles);
        }
        
        internal static string StyleContentText(string text, List<Style> styles)
        {
            if (styles.Count() == 0) text.Replace("\u001b[999m", GetStyleString(BaseStyles));
            else text.Replace("\u001b[999m", GetStyleString(styles));
            return $"{GetStyleString(styles)}{text}\u001b[0m{GetStyleString(BaseStyles)}";
        }
        internal static string StyleContentText(string text)
        {
            text = text.Replace("\u001b[999m", GetStyleString(BaseStyles));
            return text;
        }
        public static string StyleText(string text, Style style)
        {
            return $"{GetStyleString(style)}{text}\u001b[0m\u001b[999m";
        }
        public static string StyleText(string text, List<Style> styles)
        {
            return $"{GetStyleString(styles)}{text}\u001b[0m\u001b[999m";
        }
    }
    public enum Style
    {
        Reset = 0,
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
    }*/
}
