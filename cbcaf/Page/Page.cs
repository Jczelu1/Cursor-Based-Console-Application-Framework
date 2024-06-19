using cbcaf.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cbcaf.Page
{
    public class Page
    {
        public string? Id;
        public char? Group;
        public Division Body = new Division([]);

        public Procedure? OnOpen;
        public Procedure? OnClose;
        public Procedure? OnDisplay;


        public int Cursor { get; private set; }

        public Page(List<IContent> contents, string? id = null, char? group = null, Procedure? onOpen = null, Procedure? onClose = null, Procedure? onDisplay = null)
        {
            Body.Contents = contents;
            Id = id;
            Group = group;
            OnOpen = onOpen;
            OnClose = onClose;
            OnDisplay = onDisplay;
        }
        public Page(string? id = null, char? group = null, Procedure? onOpen = null, Procedure? onClose = null, Procedure? onDisplay = null) 
        {
            Id = id;
            Group = group;
            OnOpen = onOpen;
            OnClose = onClose;
            OnDisplay = onDisplay;
        }

        public int DefaultCursor()
        {
            if (Body.Contents == null) return 0;
            int i = 0;

            while (i <= Body.Contents.Count - 1)
            {
                if (Body.Contents[i] is ISelectable t && t.IsSelectable)
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
            if (Body.Contents == null) return;
            if (position == Cursor) return;
            bool succes = false;
            int i = 0;
            while (position + i <= Body.Contents.Count - 1)
            {
                if (Body.Contents[position + i] is ISelectable t && t.IsSelectable)
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
                    if (Body.Contents[position + i] is ISelectable t && t.IsSelectable)
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
            if (Body.Contents == null) return;
            int i = 1;
            while (Cursor + i <= Body.Contents.Count - 1)
            {
                if (Body.Contents[Cursor + i] is ISelectable t && t.IsSelectable)
                {
                    Cursor += i;
                    Display();
                    break;
                }
                i++;
            }
        }

        public void CursorUp()
        {
            if (Body.Contents == null) return;
            int i = -1;
            while (Cursor + i >= 0)
            {
                if (Body.Contents[Cursor + i] is ISelectable t && t.IsSelectable)
                {
                    Cursor += i;
                    Display();
                    break;
                }
                i--;
            }
        }

        public void ExecCursor()
        {
            if (Body.Contents == null) return;
            if (Body.Contents.Count > Cursor)
            {
                if (Body.Contents[Cursor] is IExecutable e)
                    e.Execute();
            }
        }
        public void Display()
        {
            Console.SetCursorPosition(0,0);
            Console.Clear();

            OnDisplay?.Invoke();

            DisplayContents();
        }
        public void DisplayContents()
        {
            int i = 0;
            foreach (IContent c in Body.Contents)
            {
                if(Cursor == i && c is ISelectable s)
                {
                    s.PrintContentSelected();
                }
                else if(c is IPrintable p)
                {
                    p.PrintContent();
                }
                i++;
            }
        }
        
    }
}
