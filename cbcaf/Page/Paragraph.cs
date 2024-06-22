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

        public Paragraph(string text = "", string? id = null, char? group = null, bool isSelectable = true) : base(text, id, group, isSelectable) { }
        public Paragraph(List<Style> textStyles, List<Style> selectedTextStyles, string text = "", string? id = null, char? group = null, bool isSelectable = true) : base(text, id, group, isSelectable) 
        {
            TextStyles = textStyles;
            SelectedTextStyles = selectedTextStyles;
        }
        public override void PrintContent(int width, int leftOffset)
        {
            Console.WriteLine(ContentStyle.StyleContentText(Text, TextStyles));
        }
        public override void PrintContentSelected(int width, int leftOffset)
        {
            Console.WriteLine(ContentStyle.GetCursor() + ContentStyle.StyleContentText(Text, SelectedTextStyles));
        }
    }
}
