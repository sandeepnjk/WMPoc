using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoreFramework.Models;
using System.Reflection;

namespace CoreFramework.Extractors
{
    class FieldExtractor : iExtractor
    {
        public ComplexTypeModel extract(ComplexTypeModel compTypeAtHand, Type classAtHand)
        {
            Dictionary<string, FieldModel> allFieldsInComplexType = compTypeAtHand.getAllFieldsInThisComplexType();
            FieldInfo[] fieldInfo = classAtHand.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            //Console.WriteLine("Length of Fields is " + fieldInfo.Length);

            for (int i = 0; i < fieldInfo.Length; i++)
            {
                string tmpFieldName = "";
                Type tmpFieldType;

                tmpFieldName = fieldInfo[i].Name;
                tmpFieldType = fieldInfo[i].FieldType;
                FieldModel fieldAtHand = new FieldModel(tmpFieldName, tmpFieldType);
                allFieldsInComplexType.Add(fieldAtHand.getFieldName(), fieldAtHand);
            }
            //Console.WriteLine("Added complex type " + compTypeAtHand.getActualTypeName());
            return compTypeAtHand;
        }
    }
}
