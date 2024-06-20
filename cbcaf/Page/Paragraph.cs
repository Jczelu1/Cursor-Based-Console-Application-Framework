using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public class Paragraph : PlainText
    {
        public List<Style> TextStyles = new List<Style>();
        public List<Style> SelectedTextStyles = new List<Style>();

        public override void PrintContent()
        {
            Console.WriteLine(ContentStyle.StyleContentText(Text, TextStyles));
        }
        public override void PrintContentSelected()
        {
            Console.WriteLine(ContentStyle.GetCursor() + ContentStyle.StyleContentText(Text, SelectedTextStyles));
        }
    }
}
