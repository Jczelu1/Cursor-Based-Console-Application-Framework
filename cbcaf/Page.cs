using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace cbcaf
{
    public class Page
    {
        public string? Id;
        public char? Group;
        public List<iContent> Contents = new List<iContent>();


        public int Cursor { get; }

        public Page(List<iContent> contents, string? id = null, char? group = null)
        {
            Contents = contents;
        }
        public Page(string? id = null, char? group = null) { }

        public void Display()
        {

        }
    }
}
