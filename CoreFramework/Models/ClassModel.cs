using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CoreFramework.Models
{
    public class ClassModel : iModel
    {
        private string className;
        private DLLModel dllThisClassBelongsTo;
        private string httpRouteMapping;
        private string aliasName;
        private SortedList<string, MethodModel> allMethodsInThisClass = new SortedList<string, MethodModel>();
        private SortedList<string, MethodModel> userSelectedMethodsInThisClass = new SortedList<string, MethodModel>();

        public ClassModel(string className)
        {
            //className = className.Replace(".", "_");
            this.className = className;
        }

        public SortedList<string, MethodModel> getAllMethodsInThisClass()
        {
            return allMethodsInThisClass;
        }

        public SortedList<string, MethodModel> getUserSelectedMethodsInThisClass()
        {
            return userSelectedMethodsInThisClass;
        }

        public string getAliasName()
        {
            return this.aliasName;
        }

        public void setAliasName(string aliasName)
        {
            this.aliasName = aliasName;
        }

        public string getClassName()
        {
            return className;
        }

        public string getHttpRouteMapping()
        {
            return httpRouteMapping;
        }

        public void setHttpRouteMapping(string httpRoute)
        {
            this.httpRouteMapping = httpRoute;
        }

        public void setDllThisClassBelongsTo(DLLModel dllAtHand)
        {
            this.dllThisClassBelongsTo = dllAtHand;
        }

        public DLLModel getDllThisClassBelongsTo()
        {
            return this.dllThisClassBelongsTo;
        }

        public string generateCodeForControllerClass()
        {
            string controllerName = (this.getAliasName() != null) ? this.getAliasName() : this.getClassName().Replace(".", "_");
            StringBuilder tempStringForCode = new StringBuilder();
            tempStringForCode.AppendLine("public class  " + controllerName + "Controller : ApiController {");
                
            SortedList<string, MethodModel> methodsInClass = this.getUserSelectedMethodsInThisClass();
            foreach (KeyValuePair<string, MethodModel> methodPair in methodsInClass)
            {
                tempStringForCode.Append(((MethodModel) methodPair.Value).generateCodeForMethod());
            }
            tempStringForCode.AppendLine("}");
            return tempStringForCode.ToString();
        }

        public XmlReader generateXml()
        {
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.Indent = true;
            XmlWriter classWriter = XmlWriter.Create(stream, writerSettings);

            classWriter.WriteStartElement("class");
            classWriter.WriteElementString("className", this.getClassName().Replace(".", "_"));
            classWriter.WriteElementString("controllerAlias", (this.getAliasName() != null) ? this.getAliasName() : "Not Set");

            SortedList<string, MethodModel> methodsInClass = this.getUserSelectedMethodsInThisClass();
            classWriter.WriteStartElement("methods");

            foreach (KeyValuePair<string, MethodModel> methodPair in methodsInClass)
            {
                MethodModel methodAtHand = methodPair.Value;
                classWriter.WriteNode(methodAtHand.generateXml(), false);
            }

            classWriter.WriteEndElement();
            classWriter.WriteEndElement();
            classWriter.Flush();
            stream.Position = 0;


            stream.Position = 0;
            XmlReader xmlReader = XmlReader.Create(stream);
            classWriter.Close();
            return xmlReader;
        }
    }
}
