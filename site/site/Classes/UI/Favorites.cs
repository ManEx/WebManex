using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace site.Classes.UI
{
    public class Favorites
    {
        
        
        public static Dictionary<string, string> GetFavs(HttpResponseBase Response)
        {
            Dictionary<string, string> favs = new Dictionary<string, string>();
            HttpCookie testCookie = new HttpCookie("favorites");
            testCookie.Expires = DateTime.Now.AddHours(24);
            testCookie.Name = "favorites";
            testCookie.Values.Add("linkText", "linkUrl");
            testCookie.Values.Add("linkText2", "linkUrl2");
            Response.Cookies.Add(testCookie);
            

            System.Web.HttpCookie cookieFavs = Response.Cookies.Get("favorites");

            if (cookieFavs.Values.Count > 0)
            {
                NameValueCollection nvc = cookieFavs.Values;
                for (int i = 0; i < nvc.Count; i++ )
                {
                    string key = nvc.Keys[i];
                    string value = nvc[i];
                    favs.Add(Resources.Localizer.GetString("Resources.Shared.Nav", key, key), value);
                }
            }
            else // get from db
            {

            }

            return favs;
        }
    }
}