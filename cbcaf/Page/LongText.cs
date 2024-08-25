using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public enum LongText
    {
        Cut,
        Truncate,
        FixedWrap,
        Wrap,
    }
    public static class LongTextUtil
    {
        public static List<string> Cut(string text, int width)
        {
            List<string> lines = new List<string>();
            int startIndex = 0;

            while (startIndex < text.Length)
            {
                // Find the next newline character
                int newlineIndex = text.IndexOf('\n', startIndex);

                if (newlineIndex == -1)
                {
                    // No more newlines, add the rest of the text
                    newlineIndex = text.Length;
                }

                // Get the current line segment
                string lineSegment = text.Substring(startIndex, newlineIndex - startIndex).TrimEnd();

                if (lineSegment.Length > width)
                {
                    lines.Add(lineSegment.Substring(0, width));
                }
                else
                {
                    lines.Add(lineSegment);
                }

                // Add any remaining part of the line segment

                // Move to the start of the next line
                startIndex = newlineIndex + 1;
            }

            return lines;
        }
        public static List<string> Truncate(string text, int width)
        {
            List<string> lines = new List<string>();
            int startIndex = 0;

            while (startIndex < text.Length)
            {
                // Find the next newline character
                int newlineIndex = text.IndexOf('\n', startIndex);

                if (newlineIndex == -1)
                {
                    // No more newlines, add the rest of the text
                    newlineIndex = text.Length;
                }

                // Get the current line segment
                string lineSegment = text.Substring(startIndex, newlineIndex - startIndex).TrimEnd();


                // Cut the line segment to the specified width and add to the list
                if (lineSegment.Length > width)
                {
                    lines.Add(lineSegment.Substring(0, width - 3).TrimEnd() + "...");
                }
                else
                {
                    lines.Add(lineSegment);
                }

                // Move to the start of the next line
                startIndex = newlineIndex + 1;
            }

            return lines;
        }
        public static List<string> FixedWrap(string text, int width)
        {
            List<string> wrappedLines = new List<string>();
            string[] paragraphs = text.Split('\n');

            foreach (string paragraph in paragraphs)
            {
                int startIndex = 0;

                while (startIndex < paragraph.Length)
                {
                    // Determine the maximum length of the current segment
                    int endIndex = Math.Min(startIndex + width, paragraph.Length);

                    // Extract the line segment and trim any leading or trailing whitespace
                    string lineSegment = paragraph.Substring(startIndex, endIndex - startIndex).TrimEnd();

                    // Add the line segment to the list
                    wrappedLines.Add(lineSegment);

                    // Move to the start of the next segment
                    startIndex = endIndex;

                    // Skip the wrap character and any additional spaces after the line break
                    while (startIndex < paragraph.Length && (paragraph[startIndex] == ' '))
                    {
                        startIndex++;
                    }
                }

                // Add an empty line to maintain the paragraph break
                //wrappedLines.Add(string.Empty);
            }

            // Remove the last empty line added
            //if (wrappedLines.Count > 0 && wrappedLines[wrappedLines.Count - 1] == string.Empty)
            //{
            //    wrappedLines.RemoveAt(wrappedLines.Count - 1);
            //}

            return wrappedLines;
        }
        public static List<char> WrapOnChar = [ ' ', '/', '\\', ')', ']', '}', '.', '?', '!', '+', '\t'];

        public static List<string> Wrap(string text, int width)
        {
            List<string> wrappedLines = new List<string>();
            string[] paragraphs = text.Split('\n');

            foreach (string paragraph in paragraphs)
            {
                int startIndex = 0;

                while (startIndex < paragraph.Length)
                {
                    // Determine the maximum length of the current segment
                    int endIndex = Math.Min(startIndex + width, paragraph.Length);

                    if(endIndex != paragraph.Length)
                    {
                        // Look for the last wrap character within the current segment to break the line at a word boundary
                        int lastWrapCharIndex = -1;
                        for (int i = endIndex - 1; i >= startIndex; i--)
                        {
                            if (WrapOnChar.Contains(paragraph[i]))
                            {
                                lastWrapCharIndex = i;
                                break;
                            }
                        }
                        if (lastWrapCharIndex != -1 || lastWrapCharIndex >= startIndex)
                        {
                            // No wrap character found, or wrap character is before the current segment
                            endIndex = lastWrapCharIndex;
                        }
                    }
                    // Extract the line segment and trim any leading or trailing whitespace
                    string lineSegment = paragraph.Substring(startIndex, endIndex - startIndex).TrimEnd();

                    // Add the line segment to the list
                    wrappedLines.Add(lineSegment);

                    // Move to the start of the next segment
                    startIndex = endIndex;

                    // Skip the wrap character and any additional spaces after the line break
                    while (startIndex < paragraph.Length && (paragraph[startIndex] == ' '))
                    {
                        startIndex++;
                    }
                }

                // Add an empty line to maintain the paragraph break
                //wrappedLines.Add(string.Empty);
            }

            // Remove the last empty line added
            //if (wrappedLines.Count > 0 && wrappedLines[wrappedLines.Count - 1] == string.Empty)
            //{
            //    wrappedLines.RemoveAt(wrappedLines.Count - 1);
            //}

            return wrappedLines;
        }
        public static List<string> GetLongText(string text, int width, LongText longTextOpt)
        {
            switch (longTextOpt)
            {
                case LongText.Cut: return Cut(text, width);
                case LongText.Truncate: return Truncate(text, width);
                case LongText.FixedWrap: return FixedWrap(text, width);
                case LongText.Wrap: return Wrap(text, width);
                default: return Cut(text, width);
            }
        }
    }
}
