using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CoreFramework.Utils
{
    class PropertyUtil
    {

        public static char methodAndAliasSeparator = '~';
        public static char aliasSeparator = '.';
        public static string getSelectedDllFile()
        {
            return ConfigurationManager.AppSettings["dllFilePath"];
        }

        public static string getUserSelectedMethods()
        {
            return ConfigurationManager.AppSettings["methodsToAdd"];
        }

        public static string getProperty(string propertyName)
        {
            return ConfigurationManager.AppSettings[propertyName];
        }
    }
}
