using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public static class AlignUtil
    {
        public static string AlignLeft(string text, int width)
        {
            int rightPadding = width - text.Length;

            return text + new string(' ', rightPadding);
        }
        public static string AlignCenter(string text, int width)
        {
            int totalPadding = width - text.Length;
            int leftPadding = totalPadding / 2;
            int rightPadding = totalPadding - leftPadding;

            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
        }
        public static string AlignRight(string text, int width)
        {
            int leftPadding = width - text.Length;

            return new string(' ', leftPadding) + text;
        }
        public static string GetAlign(string text, int width, Align align)
        {
            switch (align)
            {
                case Align.Left: return AlignLeft(text, width);
                case Align.Right: return AlignRight(text, width);
                case Align.Center: return AlignCenter(text, width);
                default: return AlignLeft(text, width);
            }
        }
    }
    public enum Align
    {
        Left,
        Center,
        Right,
    }
}
