using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreFramework.Utils;

namespace CoreFramework.Models
{
    public class ParameterModel : iModel
    {
        private MethodModel methodThisParameterBelongsTo;
        private string parameterName;
        
        //If the param type is complex the name of the param will have "obj" appended to it
        // hence using a field as against hard coding in order to be able to customize
        private string parameterCompTypeName;
        //place holder for actual type in case type is changed to string if its complex
        private Type actualType;
        private Type typeOfParameter;
        private int positionOfParameter;
        private bool isComplexType;
        //private bool isConstructorParameter;

        public ParameterModel(string parameterName, Type typeOfParameter, int positionOfParameter, MethodModel methodThisParameterBelongsTo)
        {
            this.parameterName = parameterName;
            this.typeOfParameter = typeOfParameter;
            this.actualType = typeOfParameter;
            this.positionOfParameter = positionOfParameter;
            this.methodThisParameterBelongsTo = methodThisParameterBelongsTo;
            this.isComplexType = TypeUtil.isComplexType(actualType);

            //Console.WriteLine("Complex Type encountered " + this.parameterName + " "
            //        + this.typeOfParameter + " " + this.parameterCompTypeName + " " + this.isParameterComplex() + " " + typeOfParameter);
            //if (this.isComplexType)
            {
                this.parameterName += "TO";
                //Set type to string
                this.typeOfParameter = this.parameterName.GetType();
                this.parameterCompTypeName = this.parameterName + "AsObj";
                //Console.WriteLine("Complex Type encountered " + this.parameterName + " " 
                //    + this.typeOfParameter + " " + this.parameterCompTypeName + " " + this.isParameterComplex());
            }
        }

        public string getParameterName()
        {
            return parameterName;
        }

        public string getParameterCompTypeName()
        {
            return parameterCompTypeName;
        }

        public Type getTypeOfParameter()
        {
            return typeOfParameter;
        }

        public int getPositionOfParameter()
        {
            return this.positionOfParameter;
        }

        public void setPositionOfParameter(int positionOfParameter)
        {
            this.positionOfParameter = positionOfParameter;
        }

        public string generateXMLForStructure()
        {
            StringBuilder xmlStructure = new StringBuilder();

            return xmlStructure.ToString();
        }

        private bool determineIfParamIsComplex(Type typeOfParameter)
        {
            bool isParamComplexType = false;
            if (!TypeUtil.isComplexType(typeOfParameter))
            {
                isParamComplexType = true;
            }
            return isParamComplexType;
        }

        public bool isParameterComplex() {
            return this.isComplexType;
        }

        public Type getActualType()
        {
            return this.actualType;
        }

        public XmlReader generateXml()
        {
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.Indent = true;

            XmlWriter paramWriter = XmlWriter.Create(stream, writerSettings);
            paramWriter.WriteStartDocument();

            paramWriter.WriteStartElement("parameter");

            paramWriter.WriteElementString("parameterName", this.getParameterName());
            paramWriter.WriteElementString("positionOfParameter", this.getPositionOfParameter() + "");
            paramWriter.WriteElementString("parameterType", this.getTypeOfParameter() + "");

            paramWriter.WriteEndElement();
            paramWriter.WriteEndDocument();
            paramWriter.Flush();
            paramWriter.Close();

            stream.Position = 0;
            XmlReader xmlReader = XmlReader.Create(stream);
            return xmlReader;
        }
    }
}
