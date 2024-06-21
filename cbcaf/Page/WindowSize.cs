using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public static class WindowSize
    {
        public static int StartWidth { get; set; } = 128;
        public static int StartHeight { get; set; } = 32;
        public static int MinWidth { get; set; } = 64;
        public static int MinHeight { get; set; } = 16;
        public static int? MaxWidth { get; set; } = null;
        public static int? MaxHeight { get; set; } = null;

        public static int CurrentWidth { get; set; } = 0;
        public static int CurrentHeight { get; set; } = 0;

        internal static void WindowSizeCheck()
        {
            int newWidth = Console.WindowWidth;
            int newHeight = Console.WindowHeight;

            if (Console.WindowWidth < MinWidth)
            {
                newWidth = MinWidth;
            }
            else if (MaxWidth != null && Console.WindowWidth > MaxWidth)
            {
                newWidth = MaxWidth ?? 10;
            }

            if (Console.WindowHeight < MinHeight)
            {
                newHeight = MinHeight;
            }
            else if (MaxHeight != null && Console.WindowHeight > MaxHeight)
            {
                newHeight = MaxHeight ?? 10;
            }

            if (newWidth != Console.WindowWidth || newHeight != Console.WindowHeight)
            {
                Console.SetWindowSize(newWidth, newHeight);
            }
            CurrentWidth = newWidth;
            CurrentHeight = newHeight;
        }
    }
}
