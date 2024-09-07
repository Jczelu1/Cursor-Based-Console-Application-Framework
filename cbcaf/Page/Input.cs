using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public interface IInput : IExecutable
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

        public Procedure? OnExecute;

        public int MarginTop { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        public int MarginRight { get; set; }

        private int Width = 0;
        private int LeftOffset = 0;

        public Input(string label = "", string value = "", string? id = null, char? group = null, bool isSelectable = true, CharCheck? charCheck = null, Procedure? onExecute = null, int marginTop = 0, int marginBottom = 0, int marginLeft = 0, int marginRight = 0)
        {
            Id = id;
            Group = group;
            Label = label;
            Value = value;
            InputCursor = 0;
            IsValidChar = charCheck;
            IsSelectable = isSelectable;
            OnExecute = onExecute;
            MarginTop = marginTop;
            MarginBottom = marginBottom;
            MarginLeft = marginLeft;
            MarginRight = marginRight;
        }

        public void AddChar(char c)
        {
            if (IsValidChar == null || IsValidChar(c))
            {
                Value = Value.Insert(InputCursor, c.ToString());
                InputCursor++;
            }
            Console.Write(c.ToString());
        }
        public void RemoveChar()
        {
            if (InputCursor > 0)
            {
                InputCursor--;
                Value = Value.Remove(InputCursor, 1);
            }

            Console.SetCursorPosition(Console.CursorLeft-1, Console.CursorTop);
            Console.Write(' ');
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
        public void Left()
        {
            if(InputCursor > 0)
                InputCursor--;
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
        public void Right()
        {
            if(InputCursor < Value.Length)
                InputCursor++;
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
        public void PrintContent(int width, int leftOffset)
        {
            Console.Write(new string('\n', MarginTop));
            leftOffset += MarginLeft;
            if (leftOffset < 0) leftOffset = 0;
            width -= MarginRight;
            List<string> pText = LongTextUtil.FixedWrap(Label + Value, width);
            Console.SetCursorPosition(leftOffset, Console.CursorTop);
            foreach (string line in pText)
            {
                Console.SetCursorPosition(leftOffset, Console.CursorTop);
                Console.WriteLine(line);
            }
            Console.Write(new string('\n', MarginBottom));
        }
        public void PrintContentSelected(int width, int leftOffset)
        {
            Console.Write(new string('\n', MarginTop));
            leftOffset += MarginLeft;
            if (leftOffset < 0) leftOffset = 0;
            width -= MarginRight;
            List<string> pText = LongTextUtil.FixedWrap(Cursor.CursorString+Label + Value, width);
            Console.SetCursorPosition(leftOffset, Console.CursorTop);
            foreach (string line in pText)
            {
                Console.SetCursorPosition(leftOffset, Console.CursorTop);
                Console.WriteLine(line);
            }
            Console.Write(new string('\n', MarginBottom));
        }
        public Position PrintInputSelected(int width, int leftOffset)
        {
            Width = width;
            LeftOffset = leftOffset;
            Console.Write(new string('\n', MarginTop));
            if (leftOffset < 0) leftOffset = 0;
            
            Position position = new Position();
            List<string> pText = LongTextUtil.FixedWrap(Cursor.CursorString + Label + Value, width);
            Console.SetCursorPosition(leftOffset, Console.CursorTop);
            int i = 0;
            foreach (string line in pText)
            {
                Console.SetCursorPosition(leftOffset, Console.CursorTop);
                Console.Write(line);
                if(i == pText.Count - 1)
                {
                    position.Top = Console.CursorTop;
                    position.Left = Console.CursorLeft;
                }
                Console.Write("\n");
                i++;
            }

            Console.Write(new string('\n', MarginBottom));
            InputCursor = Value.Length;

            return position;
        }
        public void Execute()
        {
            OnExecute?.Invoke();
        }
    }
}
