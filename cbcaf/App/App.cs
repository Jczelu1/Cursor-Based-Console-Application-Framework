using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using cbcaf.Page;
using Microsoft.VisualBasic;

namespace cbcaf.App
{
    public class App
    {
        public List<Page.Page> Pages = [];
        public Page.Page CurrentPage { get; private set; }
        public int CurrentPageIndex { get; private set; }

        public List<PageHistory> DisplayHistory = [];
        public int Indicator { get; private set; }

        public List<Control> Controls = [];

        public List<Control> DefaultControls = [];

        public bool Exit = false;

        public App() 
        {
            Pages = [new Page.Page()];
            CurrentPage = Pages[0];
            CurrentPageIndex = 0;
            SetDefaultControls();
        }
        public App(List<Page.Page> pages)
        {
            Pages = pages;
            if (Pages.Count > 0)
            {
                CurrentPage = pages[0];
                CurrentPageIndex = 0;
            }
            else
            {
                Pages.Add(new Page.Page());
                CurrentPage = Pages[0];
                CurrentPageIndex = 0;
            }
            SetDefaultControls();
        }
        private void SetDefaultControls()
        {
            DefaultControls =
            [
                new Control(ConsoleKey.UpArrow, ()=>{ CurrentPage.CursorUp(); }),
                new Control(ConsoleKey.DownArrow, ()=>{ CurrentPage.CursorDown(); }),
                new Control(ConsoleKey.Escape, ()=>{ Back(); }),
                new Control(ConsoleKey.Tab, ()=>{ Forward(); }),
                new Control(ConsoleKey.Enter, ()=>{ CurrentPage.ExecCursor(); }),
            ];
            Controls = DefaultControls;
        }

        public void RunApp()
        {
            Console.CursorVisible = false;
            DisplayHistory.Clear();
            Indicator = -1;
            OpenPage(0);
            while (!Exit)
            {
                ExecuteKey();

                CurrentPage.Display();
            }
        }
        public void ExitApp()
        {
            Exit = true;
        }
        public void OpenPage(int index)
        {
            if (index < 0 || index >= Pages.Count) return;
            //old page
            if (Indicator >=0)
            {
                CurrentPage.OnClose?.Invoke();
                DisplayHistory[Indicator] = GetCurrentPageHistory();
            }
            //Remove after indicator
            int i = Indicator + 2;
            while (i < DisplayHistory.Count)
            {
                DisplayHistory.RemoveAt(i);
            }
            //new page
            Indicator++;
            CurrentPageIndex = index;
            CurrentPage = Pages[index];

            CurrentPage.SetDefaultCursor();

            if (Indicator >= DisplayHistory.Count)
            {
                DisplayHistory.Add(GetCurrentPageHistory());
            }
            else
            {
                DisplayHistory[Indicator] = GetCurrentPageHistory();
            }
            CurrentPage.OnOpen?.Invoke();
            CurrentPage.Display();
        }
        public void OpenPage(string id)
        {
            int index = GetPageIndex(id);
            OpenPage(index);
        }
        public void Back()
        {
            if (Indicator != 0)
            {
                //old page
                CurrentPage.OnClose?.Invoke();
                DisplayHistory[Indicator] = GetCurrentPageHistory();
                //new page
                Indicator--;
                CurrentPageIndex = DisplayHistory[Indicator].Index;
                CurrentPage = Pages[CurrentPageIndex];
                CurrentPage.SetCursor(DisplayHistory[Indicator].Cursor);
                CurrentPage.OnOpen?.Invoke();
                CurrentPage.Display();
            }
        }
        public void Forward()
        {
            if (Indicator + 1 < DisplayHistory.Count)
            {
                //old page
                CurrentPage.OnClose?.Invoke();
                DisplayHistory[Indicator] = GetCurrentPageHistory();
                //new page
                Indicator++;
                CurrentPageIndex = DisplayHistory[Indicator].Index;
                CurrentPage = Pages[CurrentPageIndex];
                CurrentPage.SetCursor(DisplayHistory[Indicator].Cursor);
                CurrentPage.OnOpen?.Invoke();
                CurrentPage.Display();
            }
        }
        private void ExecuteKey()
        {
            ConsoleKey key = KeyReader.GetKey().Key;
            Controls.FindAll(ctrl => ctrl.ConsoleKey == key).ForEach(ctrl => ctrl.OnPress());
        }

        public int GetPageIndex(string id)
        {
            int index = Pages.FindIndex(a => a.Id == id);
            return index;
        }


        public PageHistory GetCurrentPageHistory()
        {
            return new PageHistory(CurrentPageIndex, CurrentPage.Cursor);
        }
    }
    public struct PageHistory(int index, int cursor)
    {
        public int Index { get; set; } = index;
        public int Cursor { get; set; } = cursor;
    }
}
