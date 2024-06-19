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

        public List<Style> Styles { get; set; }
        public Align align { get; set; }

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

        public void PrintDivision(int leftOffset, int topOffset, int? cursor)
        {
            //code here
        }

        public T? GetContentById<T>(string id) where T : class, IContent
        {
            return Contents.Find(c => c is T t && c.Id == id) as T;
        }
        public List<T> GetAllContentsById<T>(string id) where T : class, IContent
        {
            return Contents.FindAll(c => c.Id == id).OfType<T>().ToList();
        }
        public T? GetFirstContentByGroup<T>(char group) where T : class, IContent
        {
            return Contents.Find(c => c is T t && c.Group == group) as T;
        }
        public List<T> GetContentsByGroup<T>(char group) where T : class, IContent
        {
            return Contents.FindAll(c => c.Group == group).OfType<T>().ToList();
        }
    }
}
