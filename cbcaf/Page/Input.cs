using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public interface IInput : IPrintable
    {
        public string Value { get; set; }
        public int InputCursor { get; set; }


        public Position PrintContentSelected(int width, int leftOffset);
        public void AddChar(char c);
        public void RemoveChar();
        public void Left();
        public void Right();
    }
}
