using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public class Paragraph : PlainText
    {
        public List<Style> styles = new List<Style>();

        public override void PrintContentSelected()
        {
            Console.WriteLine(ContentStyle.GetCursor() + ContentStyle.StyleText(Text, styles));
        }
    }
}
