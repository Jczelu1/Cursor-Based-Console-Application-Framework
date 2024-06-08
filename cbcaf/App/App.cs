using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cbcaf.Page;
using Microsoft.VisualBasic;

namespace cbcaf.App
{
    public class App
    {
        public List<Page.Page> Pages = new List<Page.Page>();
        public Page.Page CurrentPage { get; private set; }
        public int CurrentPageIndex { get; private set; }

        public List<PageHistory> DisplayHistory = new List<PageHistory>();
        public int Indicator { get; private set; }

        //public List<Control> Controls = new List<Control>();

        public bool Exit = false;

        public App() 
        {
            Pages = new List<Page.Page>();
            Pages.Add(new Page.Page());
            CurrentPage = Pages[0];
            CurrentPageIndex = 0;
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
        }

        public void RunApp()
        {
            DisplayHistory.Clear();
            Indicator = -1;
            OpenPage(0);
            while (!Exit)
            {
                //ExecuteKey();

                //CurrentPage.Display();
            }
        }
        public void ExitApp()
        {
            Exit = true;
        }
        public void OpenPage(int index)
        {
            if (index < 0 || index >= Pages.Count) throw new ArgumentOutOfRangeException();
            Indicator++;
            //Remove after indicator
            int i = Indicator + 1;
            while (i < DisplayHistory.Count)
            {
                DisplayHistory.RemoveAt(i);
            }
            CurrentPageIndex = index;
            CurrentPage = Pages[index];

            CurrentPage.SetDefaultCursor();

            if (Indicator >= DisplayHistory.Count)
            {
                DisplayHistory.Add(CurrentPageHistory());
            }
            else
            {
                DisplayHistory[Indicator] = CurrentPageHistory();
            }

            CurrentPage.Display();
        }
        public void Back()
        {
            if (Indicator != 0)
            {
                DisplayHistory[Indicator] = CurrentPageHistory();
                Indicator--;
                CurrentPageIndex = DisplayHistory[Indicator].Index;
                CurrentPage = Pages[CurrentPageIndex];
                CurrentPage.SetCursor(DisplayHistory[Indicator].Cursor);
                CurrentPage.Display();
            }
        }
        public void Forward()
        {
            if (Indicator + 1 < DisplayHistory.Count)
            {
                DisplayHistory[Indicator] = CurrentPageHistory();
                Indicator++;
                CurrentPageIndex = DisplayHistory[Indicator].Index;
                CurrentPage = Pages[CurrentPageIndex];
                CurrentPage.SetCursor(DisplayHistory[Indicator].Cursor);
                CurrentPage.Display();
            }
        }

        public int? GetPageIndex(string id)
        {
            int index = Pages.FindIndex(a => a.Id == id);
            if (index == -1) return null;
            return index;
        }


        public PageHistory CurrentPageHistory()
        {
            return new PageHistory(CurrentPageIndex, CurrentPage.Cursor);
        }
    }
    public class PageHistory
    {
        public int Index { get; set; }
        public int Cursor { get; set; }
        public PageHistory(int index, int cursor)
        {
            Index = index;
            Cursor = cursor;
        }
    }
}
