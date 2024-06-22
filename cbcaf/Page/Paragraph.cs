using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public class Paragraph : PlainText
    {
        public static List<Style> DefaultParagraphStyles { get; set; } = [];
        public static List<Style> DefaultSelectedParagraphStyles { get; set; } = [];

        public List<Style> TextStyles = DefaultParagraphStyles;
        public List<Style> SelectedTextStyles = DefaultSelectedParagraphStyles;

        public int MarginTop { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        public int MarginRight { get; set; }

        public Paragraph(string text = "", string? id = null, char? group = null, bool isSelectable = true, int marginTop = 0, int marginBottom = 0, int marginLeft = 0, int marginRight = 0) : base(text, id, group, isSelectable) 
        {
            MarginTop = marginTop;
            MarginBottom = marginBottom;
            MarginLeft = marginLeft;
            MarginRight = marginRight;
        }
        public override void PrintContent(int width, int leftOffset)
        {
            for (int i = 0; i < MarginTop; i++)
            {
                Console.WriteLine();
            }
            width -= leftOffset;
            string pText = ContentStyle.GetSubstringWithoutAnsi(Text, width-MarginRight);
            Console.SetCursorPosition(leftOffset+MarginLeft, Console.CursorTop);
            Console.WriteLine(ContentStyle.StyleContentText(pText));
            for (int i = 0; i < MarginBottom; i++)
            {
                Console.WriteLine();
            }
        }
        public override void PrintContentSelected(int width, int leftOffset)
        {
            for (int i = 0; i < MarginTop; i++)
            {
                Console.WriteLine();
            }
            width -= leftOffset;
            string pText = ContentStyle.GetSubstringWithoutAnsi(ContentStyle.GetCursor()+Text, width - MarginRight);
            Console.SetCursorPosition(leftOffset + MarginLeft, Console.CursorTop);
            Console.WriteLine(ContentStyle.StyleContentText(pText));
            for (int i = 0; i < MarginBottom; i++)
            {
                Console.WriteLine();
            }
        }
    }
}
