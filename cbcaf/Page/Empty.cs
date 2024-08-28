using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace cbcaf.Page
{
    public class Empty : IPrintable
    {
        public string? Id { get; set; }
        public char? Group { get; set; }

        public byte Size { get; set; }

        public Empty(byte size = 1, string? id = null, char? group = null) 
        {
            Id = id;
            Group = group;
            Size = size;
        }

        public virtual void PrintContent(int width, int leftOffset)
        {
            Console.Write(new string('\n', Size));
        }
    }
}
