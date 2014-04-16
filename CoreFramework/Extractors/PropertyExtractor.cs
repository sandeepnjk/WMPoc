using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using CoreFramework.Models;

namespace CoreFramework.Extractors
{
    class PropertyExtractor : iExtractor
    {
        public ComplexTypeModel extract(ComplexTypeModel compTypeAtHand, Type classAtHand)
        {
            Dictionary<string, PropertyModel> allPropertiesInComplexType = compTypeAtHand.getAllPropertiesInThisComplexType();
            PropertyInfo[] propertyInfo = classAtHand.GetProperties(BindingFlags.NonPublic | 
                         BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            //Console.WriteLine("Length of Properties is " + propertyInfo.Length);

            for (int i = 0; i < propertyInfo.Length; i++)
            {
                string tmpFieldName = "";
                Type tmpFieldType;

                tmpFieldName = propertyInfo[i].Name;
                tmpFieldType = propertyInfo[i].PropertyType;
                PropertyModel propertyAtHand = new PropertyModel(tmpFieldName, tmpFieldType);
                allPropertiesInComplexType.Add(propertyAtHand.getPropertyName(), propertyAtHand);
            }

            return compTypeAtHand;
        }
    }
}
