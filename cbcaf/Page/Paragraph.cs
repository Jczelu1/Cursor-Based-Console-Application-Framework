using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public class Paragraph : PlainText
    {
        //public static List<Style> DefaultParagraphStyles { get; set; } = [];
        //public static List<Style>? DefaultSelectedParagraphStyles { get; set; } = null;

        //public List<Style> TextStyles = DefaultParagraphStyles;
        //public List<Style>? SelectedTextStyles = DefaultSelectedParagraphStyles;

        public int MarginTop { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        public int MarginRight { get; set; }

        public LongText LongTextOption { get; set; }

        public Align AlignOption { get; set; }

        public Paragraph(string text = "", string? id = null, char? group = null, bool isSelectable = true, int marginTop = 0, int marginBottom = 0, int marginLeft = 0, int marginRight = 0, LongText longTextOption = LongText.Wrap, Align alignOption = Align.Left) : base(text, id, group, isSelectable) 
        {
            MarginTop = marginTop;
            MarginBottom = marginBottom;
            MarginLeft = marginLeft;
            MarginRight = marginRight;
            LongTextOption = longTextOption;
            AlignOption = alignOption;
        }
        public override void PrintContent(int width, int leftOffset)
        {
            Console.Write(new string('\n', MarginTop));

            width -= leftOffset;
            List<string> pText = LongTextUtil.GetLongText(Text, width-MarginRight, LongTextOption);
            foreach (string line in pText)
            {
                string pLine = AlignUtil.GetAlign(line, width, AlignOption);
                Console.SetCursorPosition(leftOffset + MarginLeft, Console.CursorTop);
                Console.WriteLine(pLine);
            }

            Console.Write(new string('\n', MarginBottom));
        }
        public override void PrintContentSelected(int width, int leftOffset)
        {
            Console.Write(new string('\n', MarginTop));

            width -= leftOffset;
            List<string> pText = LongTextUtil.GetLongText(Cursor.CursorString + Text, width - MarginRight, LongTextOption);
            foreach (string line in pText)
            {
                string pLine = AlignUtil.GetAlign(line, width, AlignOption);
                Console.SetCursorPosition(leftOffset + MarginLeft, Console.CursorTop);
                Console.WriteLine(pLine);
            }

            Console.Write(new string('\n', MarginBottom));
        }
    }
}
