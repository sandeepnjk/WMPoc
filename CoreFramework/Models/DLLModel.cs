using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CoreFramework.Processor;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;

using CoreFramework.Models;
using CoreFramework.Utils;

namespace CoreFramework.Models
{
    public class DLLModel : iModel
    {
        private string dllFileName;
        private bool isManaged;
        private string fullyQualifiedPath;
        private Dictionary<string, ClassModel> allClassesInDLL = new Dictionary<string, ClassModel>();
        private Dictionary<string, ClassModel> userSelectedclassesInDLL = new Dictionary<string, ClassModel>();
        private Dictionary<string, ComplexTypeModel> allComplexTypesInDLL = new Dictionary<string, ComplexTypeModel>();

        public DLLModel(string fullyQualifiedPath)
        {
            this.fullyQualifiedPath = fullyQualifiedPath;
            isManaged = DLLProcessor.checkIfDLLIsManaged(this.fullyQualifiedPath);
            dllFileName = Path.GetFileName(this.fullyQualifiedPath);
        }

        public Dictionary<string, ClassModel> getAllClassesInThisDll()
        {
            return allClassesInDLL;
        }

        public Dictionary<string, ClassModel> getUserSelectedClassesInThisDll()
        {
            return userSelectedclassesInDLL;
        }

        public Dictionary<string, ComplexTypeModel> getAllComplexTypesInDLL()
        {
            return this.allComplexTypesInDLL;
        }

        public string getFullyQualifiedPath()
        {
            return fullyQualifiedPath;
        }

       public DLLModel processDll(DLLModel dllModel)
        {

            return new DLLProcessor().processDLL(dllModel);
        }

        public bool isDLLManaged()
        {
            return this.isManaged;
        }

        public void setDllFileName(string dllFileName)
        {
            this.dllFileName = dllFileName;
        }

        public string getDllFileName()
        {
            return this.dllFileName;
        }

        public string generateCodeForDllContents()
        {
            StringBuilder codeForDllContents = new StringBuilder();
            codeForDllContents.AppendLine("using System.IO;");
            codeForDllContents.AppendLine("using System;");
            codeForDllContents.AppendLine("using Newtonsoft.Json;");
            codeForDllContents.AppendLine("using System.Web.Http;")
                .AppendLine("namespace " + this.getDllFileName() + " {");
            Dictionary<string, ClassModel> userSelectedClassesInDll = this.getUserSelectedClassesInThisDll();
            //Iterate through each class and generate a corresponding controller for it.
            foreach (KeyValuePair<string, ClassModel> pair in userSelectedClassesInDll)
            {
                codeForDllContents.Append(((ClassModel)(pair.Value)).generateCodeForControllerClass());
            }
            codeForDllContents.AppendLine("}");

            return codeForDllContents.ToString();
        }

        public XmlReader generateXml()
        {
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.Indent = true;
            string dllName = this.dllFileName;
            using (XmlWriter writer = XmlWriter.Create(PathUtil.getFolderPathForDll(dllName) + dllName + ".xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("dll");
                writer.WriteElementString("dllFileName", dllName + ".xml");
                writer.WriteElementString("fullyQualifiedPath", this.getFullyQualifiedPath());
                writer.WriteElementString("isManaged", this.isDLLManaged() + "");
                writer.WriteStartElement("userSelectedClassesInDll");
                
                Dictionary<string, ClassModel> classesInDll = this.getUserSelectedClassesInThisDll();
                foreach (KeyValuePair<string, ClassModel> pair in classesInDll)
                {
                    ClassModel classAtHand = pair.Value;
                    XmlReader xmlReaderForClass = classAtHand.generateXml();
                    
                    writer.WriteNode(xmlReaderForClass, true);
                    
                }
                writer.WriteEndElement();

                writer.WriteStartElement("allComplexTypesInDll");
                foreach (KeyValuePair<string, ComplexTypeModel> pair in this.getAllComplexTypesInDLL())
                {
                    ComplexTypeModel complexTypeAtHand = pair.Value;
                    XmlReader xmlReaderForField = complexTypeAtHand.generateXml();

                    writer.WriteNode(xmlReaderForField, true);
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }

            return null;
        }

        public void writeComplexTypesToDisk()
        {
            ObjectProcessor.setContentToString("Set from the old world!!");
            CoreFramework.ObjectProcessor.serializeToBinaryFile("c:\\play\\complexTypes", this.getAllComplexTypesInDLL());
        }



    }
}
