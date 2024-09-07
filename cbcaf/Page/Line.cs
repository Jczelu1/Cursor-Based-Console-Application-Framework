using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public class Line : IPrintable
    {
        public string? Id { get; set; }
        public char? Group { get; set; }
        public string Pattern { get; set; }

        public int MarginTop { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        public int MarginRight { get; set; }

        public Line(string pattern = "-", string? id = null, char? group = null, int marginTop = 0, int marginBottom = 0, int marginLeft = 0, int marginRight = 0) 
        {
            Pattern = pattern;
            Id = id;
            Group = group;
            MarginTop = marginTop;
            MarginBottom = marginBottom;
            MarginLeft = marginLeft;
            MarginRight = marginRight;
        }
        public virtual void PrintContent(int width, int leftOffset)
        {
            if (string.IsNullOrEmpty(Pattern)) Pattern = " ";
            Console.Write(new string('\n', MarginTop));

            leftOffset += MarginLeft;
            if(leftOffset < 0) leftOffset = 0;
            width -= MarginRight;
            
            Console.SetCursorPosition(leftOffset, Console.CursorTop);
            string pString = "";
            if (Pattern.Length == 1)
            {
                pString = new string(Pattern[0], width);
            }
            else
            {
                //pattern builder
                while (pString.Length + Pattern.Length < width)
                {
                    pString += Pattern;
                }
                int i = 0;
                while (pString.Length < width)
                {
                    if (i >= Pattern.Length) i = 0;
                    pString += Pattern[i];
                    i++;
                }
            }
            Console.WriteLine(pString);

            Console.Write(new string('\n', MarginBottom));
        }
    }
}
