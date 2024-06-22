using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cbcaf.Page
{
    public interface IContent
    {
        string? Id { get; set; }
        char? Group { get; set; }
    }
    public interface IPrintable : IContent
    {
        public void PrintContent(int width, int leftOffset);
    }
    public interface ISelectable : IPrintable
    {
        public bool IsSelectable { get; set; }
        public void PrintContentSelected(int width, int leftOffset);
    }
    public interface IExecutable : ISelectable
    {
        public void Execute();
    }
}
