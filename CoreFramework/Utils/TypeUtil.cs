using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreFramework.Utils
{
    public class TypeUtil
    {
        public static bool isComplexType(Type typeOfObject)
        {
            
            bool isParamComplexType = false;
            if (!typeOfObject.Namespace.StartsWith("System"))
            {
                isParamComplexType = true;
            }
            //Console.WriteLine("TypeUtil " + typeOfObject + " " + isParamComplexType);
            return isParamComplexType;
        }

        public static bool isCollectionType(Type typeOfObject)
        {
            bool isCollectionType = false;

            if (typeOfObject.Namespace.StartsWith("System.Collections."))
            {
                isCollectionType = true;
            }

            return isCollectionType;
        }

        public static bool isArrayType(Type typeOfObject)
        {
            bool isArrayTypeMismatchException = false; ;

            if(typeOfObject.IsArray) {
                isArrayTypeMismatchException = true;
            }

            return isArrayTypeMismatchException;
        }
    }
}
