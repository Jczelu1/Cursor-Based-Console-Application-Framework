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

        public App(string? title = null) 
        {
            Pages = [new Page.Page()];
            CurrentPage = Pages[0];
            Title = title;

            CurrentPageIndex = 0;
            SetDefaultControls();
        }
        public App(List<Page.Page> pages, string? title = null)
        {
            Pages = pages;
            Title = title;

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
                new Control(ConsoleKey.Backspace, ()=>{ CurrentPage.InputBackspace(); }),
                new Control(ConsoleKey.RightArrow, ()=>{ CurrentPage.InputRight(); }),
                new Control(ConsoleKey.LeftArrow, ()=>{ CurrentPage.InputLeft(); }),
            ];
            Controls = DefaultControls;
        }

        public void RunApp()
        {
            Console.CursorVisible = false;
            Console.Title = Title??"Console App";
            try
            {
                Console.SetWindowSize(WindowSize.StartWidth, WindowSize.StartHeight);
                if (OperatingSystem.IsWindows())
                    Console.SetBufferSize(WindowSize.StartWidth, WindowSize.StartHeight);
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
            ConsoleKeyInfo keyInfo = KeyReader.GetKey();  // Read the key information
            ConsoleKey key = keyInfo.Key;

            // Check if the current content at the cursor is an IInput and if the key is a valid character
            if (CurrentPage.Contents[CurrentPage.Cursor] is IInput input && !char.IsControl(keyInfo.KeyChar))
            {
                input.AddChar(keyInfo.KeyChar);  // Add the character to the input
                //CurrentPage.Display();
            }
            else
            {
                // Trigger controls that match the pressed key
                Controls.FindAll(ctrl => ctrl.ConsoleKey == key).ForEach(ctrl => ctrl.OnPress());
            }
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
