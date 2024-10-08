﻿using cbcaf.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace cbcaf.Page
{
    public class Page
    {
       // public static List<Style> DefaultPageStyles { get; set; } = [];
        public string? Id;
        public char? Group;
        public List<IContent> Contents = [];

        public Procedure? OnOpen;
        public Procedure? OnClose;
        public Procedure? OnDisplay;

        public int MarginLeft;
        public int MarginRight;
        public int MarginTop;
        //public int MarginBottom;

        //public List<Style> PageStyles = DefaultPageStyles;


        public int Cursor { get; private set; }

        public Page(string? id = null, char? group = null, Procedure? onOpen = null, Procedure? onClose = null, Procedure? onDisplay = null, int marginLeft = 0, int marginRight = 0, int marginTop = 0) 
        {
            Id = id;
            Group = group;
            OnOpen = onOpen;
            OnClose = onClose;
            OnDisplay = onDisplay;
            MarginLeft = marginLeft;
            MarginRight = marginRight;
            MarginTop = marginTop;
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
            //ContentStyle.ApplyBaseStyles(PageStyles);
            Console.Clear();

            //set max buffer
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Console.SetBufferSize(Console.BufferWidth, 255);

            OnDisplay?.Invoke();

            DisplayContents();

            ClearAlerts();
        }
        public void DisplayContents()
        {
            Console.CursorVisible = false;
            Position CursorPosition = new Position(0,0);
            int i = 0;
            Console.Write(new string('\n', MarginTop));
            i = 0;
            foreach (IContent c in Contents)
            {
                if(Cursor == i)
                {
                    if(c is IInput input)
                    {
                        CursorPosition = input.PrintInputSelected(WindowSize.CurrentWidth - MarginRight, MarginLeft);
                        Console.CursorVisible = true;
                    }
                    else if(c is ISelectable s)
                    {
                        CursorPosition.Top = Console.CursorTop;
                        s.PrintContentSelected(WindowSize.CurrentWidth - MarginRight, MarginLeft);
                    }
                }
                else if(c is IPrintable p)
                {
                    p.PrintContent(WindowSize.CurrentWidth - MarginRight, MarginLeft);
                }
                i++;
            }
            
            SetBuffer();
            Console.SetCursorPosition(CursorPosition.Left, CursorPosition.Top);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (Console.BufferHeight > CursorPosition.Top + (Console.WindowHeight / 2))
                    Console.SetWindowPosition(0, Math.Max((CursorPosition.Top - Console.WindowHeight / 2) - 2, 0));
                else
                {
                    Console.SetCursorPosition(0, Math.Max((CursorPosition.Top - Console.WindowHeight / 2) - 2, 0));
                }
            }   
        }
        public void SetBuffer()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.SetBufferSize(Console.BufferWidth, Math.Max(Console.CursorTop+1, Console.WindowHeight));
            }
            
        }
        public T? GetFirstContent<T>() where T : class, IContent
        {
            return Contents.Find(c => c is T) as T;
        }
        public List<T> GetAllContents<T>() where T : class, IContent
        {
            return Contents.OfType<T>().ToList();
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

        public void AddAlert(IPrintable content, AlertPosition alertPosition)
        {
            if (content == null) return;
            Alert alert = new Alert(content);
            switch (alertPosition)
            {
                case AlertPosition.Top:
                    Contents.Insert(0, alert);
                    Cursor++;
                    break;
                case AlertPosition.Bottom:
                    Contents.Add(alert);
                    break;
                case AlertPosition.AboveCursor:
                    Contents.Insert(Cursor, alert);
                    Cursor++;
                    break;
                case AlertPosition.BelowCursor:
                    Contents.Insert(Cursor+1, alert);
                    break;
            }
            Display();
        }
        public void ClearAlerts()
        {
            for(int i = 0; i < Contents.Count; i++)
            {
                if(Contents[i] is IAlert)
                {
                    if (Cursor > i) Cursor--;
                    Contents.RemoveAt(i);
                    i--;
                }
            }
        }
        public void InputBackspace()
        {
            if (Contents[Cursor] is IInput input)
            {
                input.RemoveChar();
                Display();
            }
        }
        public void InputRight()
        {
            if (Contents[Cursor] is IInput input)
            {
                input.Right();
                Display();
            }
        }
        public void InputLeft()
        {
            if (Contents[Cursor] is IInput input)
            {
                input.Left();
                Display();
            }
        }
    }
    public enum AlertPosition
    {
        Top,
        Bottom,
        AboveCursor,
        BelowCursor
    }
}
