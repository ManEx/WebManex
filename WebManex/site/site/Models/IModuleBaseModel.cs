using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Resources;
using System.Globalization;
using site.Classes.UI;

namespace site.Models
{
    public class IModuleBaseModel
    {
        public ResourceManager ResManager;
        public Module Module;
        public Dictionary<string, string> Favorites;
        public SideMenuContent SideMenuContent;
    }
}