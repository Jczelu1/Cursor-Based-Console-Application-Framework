using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cbcaf.Page;
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

        public string? Title;

        //width
        public int StartWidth { get; set; }
        public int StartHeight { get; set; }

        public int? MinWidth { get; set; }
        public int? MinHeight { get; set; }
        public int? MaxWidth { get; set; }
        public int? MaxHeight { get; set; }

        public App(string? title = null, int startWidth = 128, int startHeight = 32, int? minWidth = 10, int? minHeight = 10, int? maxWidth = null, int? maxHeight = null) 
        {
            Pages = [new Page.Page()];
            CurrentPage = Pages[0];
            Title = title;
            StartWidth = startWidth;
            StartHeight = startHeight;
            MinWidth = minWidth;
            MinHeight = minHeight;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;

            CurrentPageIndex = 0;
            SetDefaultControls();
        }
        public App(List<Page.Page> pages, string? title = null, int startWidth = 128, int startHeight = 32, int? minWidth = 10, int? minHeight = 10, int? maxWidth = null, int? maxHeight = null)
        {
            Pages = pages;
            Title = title;
            StartWidth = startWidth;
            StartHeight = startHeight;
            MinWidth = minWidth;
            MinHeight = minHeight;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;

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
            Console.Title = Title??"Console App";
            try
            {
                Console.SetWindowSize(StartWidth, StartHeight);
                if (OperatingSystem.IsWindows())
                    Console.SetBufferSize(StartWidth, StartHeight);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Invalid start window size: " + e.Message);
            }
            DisplayHistory.Clear();
            Indicator = -1;
            OpenPage(0);
            while (!Exit)
            {
                WindowSizeCheck();
                ExecuteKey();
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

        private void WindowSizeCheck()
        {
            int newWidth = Console.WindowWidth;
            int newHeight = Console.WindowHeight;

            if (MinWidth != null && Console.WindowWidth < MinWidth)
            {
                newWidth = MinWidth ?? 10;
            }
            else if (MaxWidth != null && Console.WindowWidth > MaxWidth)
            {
                newWidth = MaxWidth ?? 10;
            }

            if (MinHeight != null && Console.WindowHeight < MinHeight)
            {
                newHeight = MinHeight ?? 10;
            }
            else if (MaxHeight != null && Console.WindowHeight > MaxHeight)
            {
                newHeight = MaxHeight ?? 10;
            }

            if (newWidth != Console.WindowWidth || newHeight != Console.WindowHeight)
            {
                Console.SetWindowSize(newWidth, newHeight);
                CurrentPage.Display();
            }
        }
    }
    public struct PageHistory(int index, int cursor)
    {
        public int Index { get; set; } = index;
        public int Cursor { get; set; } = cursor;
    }
}
