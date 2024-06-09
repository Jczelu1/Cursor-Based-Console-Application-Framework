using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cbcaf.Page
{
    public interface iContent
    {
        string? Id { get; set; }
        char? Group { get; set; }
    }
    public interface iPrintable : iContent
    {
        public void PrintContent();
    }
    public interface iSelectable : iPrintable
    {
        public bool IsSelectable { get; set; }
        public void PrintContentSelected();
    }
    public interface iExecutable : iSelectable
    {
        public void Execute();
    }
}
