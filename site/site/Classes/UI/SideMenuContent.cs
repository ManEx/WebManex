using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace site.Classes.UI
{
    public class SideMenuContent
    {
        public List<SideMenuSection> Content { get; set; }
    }

    public class SideMenuSection
    {
        public string Title { get; set; }
        public Dictionary<string, string> Links { get; set; }
    }
}