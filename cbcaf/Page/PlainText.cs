using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public class PlainText : ISelectable
    {
        public string? Id { get; set; }
        public char? Group { get; set; }
        public string Text { get; set; }

        public bool IsSelectable { get; set; }

        public PlainText(string text = "", string? id = null, char? group = null, bool isSelectable = true)
        {
            Text = text;
            Id = id;
            Group = group;
            IsSelectable = isSelectable;
        }

        public virtual void PrintContent(int width, int leftOffset)
        {
            width-=leftOffset;
            string pText = ContentStyle.GetSubstringWithoutAnsi(Text,width);
            Console.SetCursorPosition(leftOffset, Console.CursorTop);
            Console.WriteLine(ContentStyle.StyleContentText(pText));
        }

        public virtual void PrintContentSelected(int width, int leftOffset)
        {
            width -= leftOffset;
            string pText = ContentStyle.GetSubstringWithoutAnsi(ContentStyle.GetCursor()+Text, width);
            Console.SetCursorPosition(leftOffset, Console.CursorTop);
            Console.WriteLine(ContentStyle.StyleContentText(pText));
        }
    }
    public class PlainTextButton : PlainText, IExecutable
    {
        public Procedure OnExecute;

        public PlainTextButton(Procedure onExecute, string text = "", string? id = null, char? group = null, bool isSelectable = true) : base(text, id, group, isSelectable)
        {
            OnExecute = onExecute;
        }
        public void Execute()
        {
            OnExecute();
        }
    }
}
