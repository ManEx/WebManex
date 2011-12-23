using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace site.Classes.UI
{
    public class Module
    {
        public string Name { get; set; }
        public string HomePage { get; set; }
        public Dictionary<string, string> Links { get; set; }
        public int Id { get; set; }
        public string IconCssClass { get; set; }
        public bool IsFavorite { get; set; }
        public string ResourceNameSpace { get; set; }
        public string ResourceKey { get; set; }
    }
}