using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreFramework.Models;

using System.IO;
using System.Dynamic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using CoreFramework.Models;
using CoreFramework.Builders;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CoreFramework
{
    public class ObjectProcessor
    {
        public static string something = "Holds Old content";

        public static Dictionary<string, Dictionary<string, ComplexTypeModel>> complexTypesAcrossDlls= 
                     new Dictionary<string, Dictionary<string, ComplexTypeModel>>();

        private static Dictionary<string, ComplexTypeModel> getComplexTypesForDll(string dllName)
        {
            Dictionary<string, ComplexTypeModel> complexTypesForDll = null;
            if(complexTypesAcrossDlls.ContainsKey(dllName)) {
                complexTypesForDll = complexTypesAcrossDlls[dllName];
            }
            //else
            //{
            //    string pathForDllBinary = @AppDomain.CurrentDomain.BaseDirectory + dllName;
            //    complexTypesForDll = getAllComplexTypesFromBinaryFile(pathForDllBinary);
            //    complexTypesAcrossDlls.Add(dllName, complexTypesForDll);
            //}

            return complexTypesForDll;            
        }

        public static Dictionary<string, Dictionary<string, ComplexTypeModel>> getComplexTypesAcrossDlls()
        {
            return complexTypesAcrossDlls;
        }

        public static Dictionary<string, ComplexTypeModel> getAllComplexTypesFromBinaryFile(string filePath)
        {
            Dictionary<string, ComplexTypeModel> objToDeserialize;
            Stream stream = File.Open(filePath, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objToDeserialize = (Dictionary<string, ComplexTypeModel>)bFormatter.Deserialize(stream);
            stream.Close();
            return objToDeserialize;
        }

        public static void serializeToBinaryFile(string filePath, object obj)
        {
            Stream stream = File.Open(filePath, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, obj);
            stream.Close();
        }

        public static Dictionary<string, ComplexTypeModel> deserializeFromBinaryFile(string filePath)
        {
            Dictionary<string, ComplexTypeModel> objToDeserialize;
            Stream stream = File.Open(filePath, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objToDeserialize = (Dictionary<string, ComplexTypeModel>)bFormatter.Deserialize(stream);
            stream.Close();
            return objToDeserialize;
        }

        public static object prepCompTypeForInvocation(string dllFile, string complexType, string jsonAsString)
        {
            ExpandoObject jsonAsObject = JsonConvert.DeserializeObject<ExpandoObject>
                                         (jsonAsString, new ExpandoObjectConverter());
            IDictionary<string, object> propertyValues = (IDictionary<string, object>) jsonAsObject;
            return prepCompTypeForInvocation(dllFile, complexType, propertyValues);
        }

        public static object prepCompTypeForInvocation(string dllFile, string complexType, 
                                                 IDictionary<string, object> jsonAsObject)
        {
            object objToReturn = null;
            Dictionary<string, ComplexTypeModel> allCompTypesForDll = getComplexTypesForDll(dllFile);
            if (allCompTypesForDll != null)
            {
                if (allCompTypesForDll.ContainsKey(complexType)) {
                    ComplexTypeModel compModelAtHand = allCompTypesForDll[complexType];
                    
                    //First build the instance of the object
                    ConstructorBuilder builder = new ConstructorBuilder();
                    objToReturn = builder.build(compModelAtHand);

                    iBuilder[] objBuilders = new iBuilder[] { new FieldBuilder() };

                    for (int i = 0; i < objBuilders.Length; i++)
                    {
                         objToReturn = objBuilders[i].build(compModelAtHand, jsonAsObject, objToReturn);
                    }


                } else {
                    //Some exception handling code here
                }                
            }
            else
            {
                //Some exception handling code here
            }
            return objToReturn;
        }

        private IDictionary<string, object> extractContentFromJson(string jsonAtHand)
        {
            ExpandoObject deserializedJson = JsonConvert.DeserializeObject<ExpandoObject>(jsonAtHand, new ExpandoObjectConverter());
            IDictionary<string, object> propertyValues = (IDictionary<string, object>) deserializedJson;
            return propertyValues;
        }

        public static void Main(string[] args)
        {
            ObjectProcessor.getAllComplexTypesFromBinaryFile("c:\\complexTypes");
        }

        public static void printWooHoo()
        {
            Console.WriteLine("WooHoo!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

        public static void setContentToString(string something)
        {
            ObjectProcessor.something = something;
        }

        public static string printContentOfString()
        {
            return something;
        }
    }
}
