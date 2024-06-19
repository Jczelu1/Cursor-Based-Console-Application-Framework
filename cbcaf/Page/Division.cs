using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public class Division : IContent
    {
        public string? Id { get; set; }
        public char? Group { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int MarginTop { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        public int MarginRight { get; set; }
        public bool FloatLeft { get; set; }

        public List<IContent> Contents { get; set; } = [];

        public Division(List<IContent> contents, int? width = null, int? height = null, int marginTop = 0, int marginBottom = 0, int marginLeft = 0, int marginRight = 0, bool floatLeft = false)
        {
            Contents = contents;
            Width = width;
            Height = height;
            MarginTop = marginTop;
            MarginBottom = marginBottom;
            MarginLeft = marginLeft;
            MarginRight = marginRight;
            FloatLeft = floatLeft;
        }

        public void PrintDivision(int leftOffset, int topOffset)
        {
            //code here
        }

    }
}
