using cbcaf.Page.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cbcaf.Page
{
    public class Page
    {
        public string Id;
        public char Group;
        public List<iContent> Contents = new List<iContent>();


        public int Cursor { get; }

        public Page(List<iContent> contents, string id = null, char group = null)
        {
            Contents = contents;
        }
        public Page(string id = null, char group = null) { }

        public int DefaultCursor()
        {
            if (Contents == null) return;
            int i = 0;

            while (i <= Contents.Count - 1)
            {
                if (Contents[i] is Text t && t.Selectable == true)
                {
                    return i;
                }
                i++;
            }
            return 0;
        }
        public void SetDefaultCursor()
        {
            Cursor = DefaultCursor();
        }

        public void SetCursor(int position)
        {
            if (Contents == null) return;
            if (position == Cursor) return;
            bool succes = false;
            int i = 0;
            while (position + i <= Contents.Count - 1)
            {
                if (Contents[position + i] is Text t && t.Selectable == true)
                {
                    Cursor = position + i;
                    succes = true;
                    break;
                }
                i++;
            }
            if (!succes)
            {
                i = 0;
                while (position + i >= 0)
                {
                    if (Contents[position + i] is Text t && t.Selectable == true)
                    {
                        Cursor = position + i;
                        succes = true;
                        break;
                    }
                    i--;
                }
            }
            if (succes)
            {
                Display();
            }
        }
        public void CursorDown()
        {
            if (Contents == null) return;
            int i = 1;
            while (Cursor + i <= Contents.Count - 1)
            {
                if (Contents[Cursor + i] is Text t && t.Selectable == true)
                {
                    Cursor = Cursor + i;
                    Display();
                    break;
                }
                i++;
            }
        }

        public void CursorUp()
        {
            if (Contents == null) return;
            int i = -1;
            while (Cursor + i >= 0)
            {
                if (Contents[Cursor + i] is Text t && t.Selectable == true)
                {
                    Cursor = Cursor + i;
                    Display();
                    break;
                }
                i--;
            }
        }

        public void ExecCursor()
        {
            if (Contents != null)
            {
                if (Contents.Count > Cursor)
                {
                    if (Contents[Cursor] is iExecutable e)
                        e.Execute();
                }
            }
        }
        public void Display()
        {
            Console.Clear();
            Console.CursorVisible = false;
            DisplayContents();

        }
        public void DisplayContents()
        {
            int i = 0;
            foreach (iContent c in Contents)
            {
                if(Cursor == i && c is iSelectable s)
                {
                    s.PrintSelected();
                }
                if(c is iPrintable p)
                {
                    p.PrintContent();
                }
                i++;
            }
        }
    }
}
