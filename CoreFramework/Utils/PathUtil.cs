using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreFramework.Utils
{
    class PathUtil
    {
        public static string getFolderPathForDll(string dllFileName) {
            return getBasePath() + dllFileName + "\\";
        }

        public static string getBasePath()
        {
            return @AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
