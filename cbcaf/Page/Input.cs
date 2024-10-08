﻿using System;
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
            Console.Write(new string('\n', MarginTop));
            leftOffset += MarginLeft;
            if (leftOffset < 0) leftOffset = 0;
            width -= leftOffset;
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
            width -= leftOffset;
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
        public void Execute()
        {
            OnExecute?.Invoke();
        }
    }
}
