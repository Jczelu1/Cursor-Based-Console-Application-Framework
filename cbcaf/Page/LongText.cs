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
                string lineSegment = text.Substring(startIndex, newlineIndex - startIndex).Trim();

                // Trim end whitespace only
                lineSegment = lineSegment.TrimEnd();

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
                string lineSegment = text.Substring(startIndex, newlineIndex - startIndex);

                // Trim end whitespace only
                lineSegment = lineSegment.TrimEnd();

                // Cut the line segment to the specified width and add to the list
                if (lineSegment.Length > width)
                {
                    lines.Add(lineSegment.Substring(0, width - 3) + "...");
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
            int startIndex = 0;

            while (startIndex < text.Length)
            {
                // Find the next newline character
                int newlineIndex = text.IndexOf('\n', startIndex);

                // Determine the end index of the current segment
                int endIndex = Math.Min(startIndex + width, text.Length);

                // If a newline is found and it's within the current segment width
                if (newlineIndex != -1 && newlineIndex < endIndex)
                {
                    endIndex = newlineIndex;
                }

                // Extract the line segment
                string lineSegment = text.Substring(startIndex, endIndex - startIndex);

                // Add the line segment to the list
                wrappedLines.Add(lineSegment);

                // Move to the start of the next segment
                startIndex = endIndex;

                // If the current segment ended with a newline, skip it
                if (startIndex < text.Length && text[startIndex] == '\n')
                {
                    startIndex++;
                }
            }

            return wrappedLines;
        }
        public static List<char> WrapOnChar = [ ' ', '/', '\\', ')', ']', '}', '.', '?', '!', '+' ];
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

                    // Look for the last space within the current segment to break the line at a word boundary
                    int lastSpaceIndex = -1;
                    foreach (char c in WrapOnChar)
                    {
                        int index = paragraph.LastIndexOf(c, endIndex - 1, endIndex - startIndex);
                        if(index>lastSpaceIndex) lastSpaceIndex = index;
                    }
                    
                    if (lastSpaceIndex == -1 || lastSpaceIndex < startIndex)
                    {
                        // No space found, or space is before the current segment
                        lastSpaceIndex = endIndex;
                    }

                    // Extract the line segment and trim any leading or trailing whitespace
                    string lineSegment = paragraph.Substring(startIndex, lastSpaceIndex - startIndex).Trim();

                    // Add the line segment to the list
                    wrappedLines.Add(lineSegment);

                    // Move to the start of the next segment
                    startIndex = lastSpaceIndex;

                    // Skip any spaces after the line break
                    while (startIndex < paragraph.Length && (paragraph[startIndex] == ' ' || paragraph[startIndex] == '\n'))
                    {
                        startIndex++;
                    }
                }

                // Add an empty line to maintain the paragraph break
                wrappedLines.Add(string.Empty);
            }

            // Remove the last empty line added
            if (wrappedLines.Count > 0 && wrappedLines[wrappedLines.Count - 1] == string.Empty)
            {
                wrappedLines.RemoveAt(wrappedLines.Count - 1);
            }

            return wrappedLines;
        }

    }
}
