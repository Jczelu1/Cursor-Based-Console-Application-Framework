using cbcaf.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cbcaf.Page
{
    public class Page
    {
        public static List<Style> DefaultPageStyles { get; set; } = [];
        public string? Id;
        public char? Group;
        public List<IContent> Contents = [];

        public Procedure? OnOpen;
        public Procedure? OnClose;
        public Procedure? OnDisplay;

        public List<Style> PageStyles = DefaultPageStyles;


        public int Cursor { get; private set; }

        public Page(string? id = null, char? group = null, Procedure? onOpen = null, Procedure? onClose = null, Procedure? onDisplay = null) 
        {
            Id = id;
            Group = group;
            OnOpen = onOpen;
            OnClose = onClose;
            OnDisplay = onDisplay;
        }
        public Page(List<IContent> contents, string? id = null, char? group = null, Procedure? onOpen = null, Procedure? onClose = null, Procedure? onDisplay = null) : this(id, group, onOpen, onClose, onDisplay)
        {
            Contents = contents;
        }
        public Page(List<IContent> contents, List<Style> pageStyles, string? id = null, char? group = null, Procedure? onOpen = null, Procedure? onClose = null, Procedure? onDisplay = null) : this(contents, id, group, onOpen, onClose, onDisplay)
        {
            PageStyles = pageStyles;
        }
        public Page(List<Style> pageStyles, string? id = null, char? group = null, Procedure? onOpen = null, Procedure? onClose = null, Procedure? onDisplay = null) : this(id, group, onOpen, onClose, onDisplay)
        {
            PageStyles = pageStyles;
        }

        public int DefaultCursor()
        {
            if (Contents == null) return 0;
            int i = 0;

            while (i <= Contents.Count - 1)
            {
                if (Contents[i] is ISelectable t && t.IsSelectable)
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
                if (Contents[position + i] is ISelectable t && t.IsSelectable)
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
                    if (Contents[position + i] is ISelectable t && t.IsSelectable)
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
                if (Contents[Cursor + i] is ISelectable t && t.IsSelectable)
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
            if (Contents == null) return;
            int i = -1;
            while (Cursor + i >= 0)
            {
                if (Contents[Cursor + i] is ISelectable t && t.IsSelectable)
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
            if (Contents == null) return;
            if (Contents.Count > Cursor)
            {
                if (Contents[Cursor] is IExecutable e)
                    e.Execute();
            }
        }
        public void Display()
        {
            WindowSize.WindowSizeCheck();
            ContentStyle.ApplyBaseStyles(PageStyles);
            Console.Clear();

            OnDisplay?.Invoke();

            DisplayContents();
        }
        public void DisplayContents()
        {
            int i = 0;
            foreach (IContent c in Contents)
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
        public T? GetContentById<T>(string id) where T : class, IContent
        {
            return Contents.Find(c => c is T t && c.Id == id) as T;
        }
        public List<T> GetAllContentsById<T>(string id) where T : class, IContent
        {
            return Contents.FindAll(c => c.Id == id).OfType<T>().ToList();
        }
        public T? GetFirstContentByGroup<T>(char group) where T : class, IContent
        {
            return Contents.Find(c => c is T t && c.Group == group) as T;
        }
        public List<T> GetContentsByGroup<T>(char group) where T : class, IContent
        {
            return Contents.FindAll(c => c.Group == group).OfType<T>().ToList();
        }
    }
}
