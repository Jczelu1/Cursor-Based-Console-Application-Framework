using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public interface IInput : ISelectable
    {
        public string Value { get; set; }

        public Position PrintInputSelected(int width, int leftOffset);
        public void AddChar(char c);
        public void RemoveChar();
        public void Left();
        public void Right();
    }
    public delegate bool CharCheck(char c);
    public class Input : IInput
    {
        public string? Id { get; set; }
        public char? Group { get; set; }

        public bool IsSelectable { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public int InputCursor { get; private set; }
        public CharCheck? IsValidChar { get; set; }

        public Input(string label = "", string value = "", string? id = null, char? group = null, bool isSelectable = true, CharCheck? charCheck = null)
        {
            Id = id;
            Group = group;
            Label = label;
            Value = value;
            InputCursor = 0;
            IsValidChar = charCheck;
            IsSelectable = isSelectable;
        }

        public void AddChar(char c)
        {
            if (IsValidChar == null || IsValidChar(c))
            {
                Value = Value.Insert(InputCursor, c.ToString());
                InputCursor++;
            }
        }
        public void RemoveChar()
        {
            if (InputCursor > 0)
            {
                InputCursor--;
                Value = Value.Remove(InputCursor, 1);
            }
        }
        public void Left()
        {
            if(InputCursor > 0)
                InputCursor--;
        }
        public void Right()
        {
            if(InputCursor < Value.Length)
                InputCursor++;
        }
        public void PrintContent(int width, int leftOffset)
        {
            width -= leftOffset;
            if (leftOffset < 0) leftOffset = 0;
            List<string> pText = LongTextUtil.FixedWrap(Label + Value, width);
            Console.SetCursorPosition(leftOffset, Console.CursorTop);
            foreach (string line in pText)
            {
                Console.SetCursorPosition(leftOffset, Console.CursorTop);
                Console.WriteLine(line);
            }
        }
        public void PrintContentSelected(int width, int leftOffset)
        {
            width -= leftOffset;
            if (leftOffset < 0) leftOffset = 0;
            List<string> pText = LongTextUtil.FixedWrap(Cursor.CursorString+Label + Value, width);
            Console.SetCursorPosition(leftOffset, Console.CursorTop);
            foreach (string line in pText)
            {
                Console.SetCursorPosition(leftOffset, Console.CursorTop);
                Console.WriteLine(line);
            }
        }
        public Position PrintInputSelected(int width, int leftOffset)
        {
            width -= leftOffset;
            if (leftOffset < 0) leftOffset = 0;
            //temporary solution
            int cursorPos = Cursor.CursorString.Length + Label.Length + InputCursor;
            Position position = new Position((cursorPos / width) + Console.CursorTop, (cursorPos % width) + leftOffset);
            
            List<string> pText = LongTextUtil.FixedWrap(Cursor.CursorString + Label + Value, width);
            Console.SetCursorPosition(leftOffset, Console.CursorTop);
            foreach (string line in pText)
            {
                Console.SetCursorPosition(leftOffset, Console.CursorTop);
                Console.WriteLine(line);
            }
            return position;
        }
    }
}
