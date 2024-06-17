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

        public void PrintContent()
        {
            Console.WriteLine(Text);
        }

        public void PrintContentSelected()
        {
            Console.WriteLine(ContentStyle.Cursor + Text);
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
