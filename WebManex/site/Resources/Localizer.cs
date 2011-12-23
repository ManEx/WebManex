using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;

namespace Resources
{
    public class Localizer
    {
        /// <summary>
        /// This is used for when you need to get a resource value from a variable (e.g, key from a database)
        /// </summary>
        /// <param name="nmSpace">should be something like: Resources.Shared.Nav</param>
        /// <param name="key"></param>
        /// <param name="defaultStr"></param>
        /// <returns></returns>
        public static string GetString(string nmSpace, string key, string defaultStr)
        {
            defaultStr = "$$" + defaultStr; // prepend $'s so we know that something is wrong, yet it will display something useful
            ResourceManager rm = new ResourceManager(nmSpace, Assembly.GetExecutingAssembly());
            string result = "";
            try
            {
                result = rm.GetString(key);
                if (String.IsNullOrWhiteSpace(result)) 
                {
                    // not in name space so try the global strings
                    rm = new ResourceManager("Resources.Global", Assembly.GetExecutingAssembly());
                    result = rm.GetString(key);
                    if (string.IsNullOrEmpty(result)) { result = defaultStr; }
                }   
            }
            catch 
            {
                result = defaultStr;
            }
            return result;
        }
    }
}
