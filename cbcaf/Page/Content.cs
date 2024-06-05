using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cbcaf.Page.Page
{
    public interface iContent
    {
        string Id { get; set; }
        char Group { get; set; }
    }
    public interface iPrintable : iContent
    {
        public void PrintContent();
    }
    public interface iSelectable : iPrintable
    {
        public bool IsSelectDisabled();
        public void PrintContentSelected();
    }
    public interface iExecutable : iContent
    {
        public void Execute();
    }
    public class PlainText : iSelectable
    {
        public string Id { get; set; }
        public char Group { get; set; }
        public string Text { get; set; }

        public PlainText(string text = "", string id = null, char group = null)
        {
            Text = "";
            Id = null;
            Group = null;
        }

        public void PrintContent()
        {
            Console.WriteLine(Text);
        }

        public void PrintContentSelected()
        {
            Console.WriteLine(">" + Text);
        }

        public bool IsSelectDisabled()
        {
            return false;
        }
    }
}
