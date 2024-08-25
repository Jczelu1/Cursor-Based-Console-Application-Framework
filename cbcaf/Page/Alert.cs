using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbcaf.Page
{
    public interface IAlert : IContent { }
    public class Alert : IAlert, IPrintable
    {
        public string? Id { get; set; }
        public char? Group { get; set; }

        public IPrintable Content { get; set; }

        public Alert(IPrintable content, string? id=null, char? group = 'A') 
        {
            Content = content;
            Id = id;
            Group = group;
        }
        public void PrintContent(int width, int leftOffset)
        {
            Content.PrintContent(width, leftOffset);
        }
    }
}
