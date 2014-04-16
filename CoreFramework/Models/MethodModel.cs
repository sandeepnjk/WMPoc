using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreFramework.Models
{
    public class MethodModel : iModel
    {
        private string shortenedMethodName;
        private ClassModel classThisMethodBelongsTo;
        private string methodName;
        private string aliasName;
        private SortedList<int, ParameterModel> allParametersInTheMethod = new SortedList<int, ParameterModel>();
        private SortedList<int, ParameterModel> userSelectedParametersInTheMethod = new SortedList<int, ParameterModel>();
        private Type methodReturnType;
        private int numberOfMethodParameters;

        public MethodModel(string methodName, ClassModel classThisMethodBelongsTo, Type methodReturnType)
        {
            this.methodName = methodName;
            this.methodReturnType = methodReturnType;
            this.classThisMethodBelongsTo = classThisMethodBelongsTo;
        }

        public SortedList<int, ParameterModel> getAllParametersInThisMethod()
        {
            return allParametersInTheMethod;
        }

        public SortedList<int, ParameterModel> getUserSelectedParametersInThisMethod()
        {
            return userSelectedParametersInTheMethod;
        } 

        public string getShortenedName()
        {
            return this.shortenedMethodName;
        }

        public string getMethodName()
        {
            return this.methodName;
        }

        public ClassModel getClassThisMethodBelongsTo()
        {
            return this.classThisMethodBelongsTo;
        }

        public Type getMethodReturnType()
        {
            return this.methodReturnType;
        }

        public int getNumberOfMethodParameters()
        {
            return this.numberOfMethodParameters;
        }

        public void setShortenedMethodName(string shortenedMethodName)
        {
            this.shortenedMethodName = shortenedMethodName;
        }

        public void setNumberOfMethodParameters(int numberOfMethodParameters)
        {
            this.numberOfMethodParameters = numberOfMethodParameters;
        }

        public string getAliasName()
        {
            return this.aliasName;
        }

        public void setAliasName(string aliasName)
        {
            this.aliasName = aliasName;
        }

        public string generateCodeForMethod()
        {
            StringBuilder codeForMethod = new StringBuilder();
            codeForMethod.Append(this.methodSignatureGenerator());
            codeForMethod.AppendLine("{");
            codeForMethod.Append(this.methodInternalContentGenerator());
            codeForMethod.AppendLine("}");
            return codeForMethod.ToString();
        }

        private string methodSignatureGenerator()
        {
            string codeForMethodSignature = "";
            string returnTypeCodeToUse = "string";

            if (this.getMethodReturnType().ToString().Contains("Void"))
            {
                returnTypeCodeToUse = "void";
            }

            codeForMethodSignature += "public " + returnTypeCodeToUse + " ";
            string methodName = (this.getAliasName() != null) ? this.getAliasName() : this.getMethodName();
            codeForMethodSignature += "Get" + methodName + " ";
            codeForMethodSignature += "(";
            if (this.getNumberOfMethodParameters() != 0)
            {
                codeForMethodSignature += this.paramListGenerator();
            }
            codeForMethodSignature += ")";
            return codeForMethodSignature;
        }

        private StringBuilder methodInternalContentGenerator()
        {
            StringBuilder codeForMethodInternals = new StringBuilder();
            
            codeForMethodInternals.Append("string assemblyName = \"")
            .Append(this.getClassThisMethodBelongsTo().getDllThisClassBelongsTo()
                .getFullyQualifiedPath().Replace("\\", "\\\\") + "\";")
            .AppendLine(this.initializeParamComplexObjs())
            .AppendLine("string className = \"" + this.getClassThisMethodBelongsTo().getClassName()
                + "\";")
            .AppendLine("System.Reflection.Assembly assembly =" +
                "System.Reflection.Assembly.LoadFile(assemblyName);")
            .AppendLine("System.Type type = assembly.GetType(className);")
            .AppendLine("string methodName = \"" + this.getMethodName() + "\";")
            .AppendLine("object classInstance =  System.Activator.CreateInstance(type);")
            .AppendLine("System.Reflection.MethodInfo methodInfo = type.GetMethod(methodName);");

            if (this.getNumberOfMethodParameters() == 0)
            {
                codeForMethodInternals.AppendLine("object result = methodInfo.Invoke(classInstance, null);");
            }
            else
            {
                codeForMethodInternals.AppendLine("object result = methodInfo.Invoke(classInstance," +
                    this.generateParamArrayForDLLInvocation() + ");");
            }

            if (!this.getMethodReturnType().ToString().Contains("Void"))
            {
                codeForMethodInternals.AppendLine("string json = JsonConvert.SerializeObject(result);");
                //codeForMethodInternals.AppendLine("return " + "(" + this.getMethodReturnType().ToString()  + ") result;");
                codeForMethodInternals.AppendLine("return json;");
            }
            
            return codeForMethodInternals;
        }

        private string initializeParamComplexObjs()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, ParameterModel> pair in this.getAllParametersInThisMethod())
            {
                ParameterModel paramAtHand = pair.Value;
                string dllFileName = this.getClassThisMethodBelongsTo().getDllThisClassBelongsTo().getDllFileName();
                if (paramAtHand.isParameterComplex())
                {
                    //Something like object sampleClassTwoTOObj 
                    //= CoreFramework.prepCompTypeForInvocation(dllFileName, complexTypeName, jsonAsString);
                    sb.AppendLine("object " + paramAtHand.getParameterCompTypeName());
                    sb.Append(" = CoreFramework.ObjectProcessor.prepCompTypeForInvocation(");
                    sb.Append("\"" + dllFileName + "\"" + ",");
                    sb.Append("\"" + paramAtHand.getActualType() + "\"" + ",");
                    sb.Append(paramAtHand.getParameterName());
                    sb.Append(");");
                }
            }

            return sb.ToString();
        }

        private string generateParamArrayForDLLInvocation()
        {
            string stringOfDLLMethodParams = "";
            stringOfDLLMethodParams += "new object[] {";
            int i = 0;
            foreach (KeyValuePair<int, ParameterModel> pair in this.getAllParametersInThisMethod())
            {
                i++;
                ParameterModel paramAtHand = pair.Value;
                if (paramAtHand.isParameterComplex())
                {
                    stringOfDLLMethodParams += paramAtHand.getParameterCompTypeName();
                }
                else
                {
                    stringOfDLLMethodParams += paramAtHand.getParameterName();
                }
                
                if (i != this.getNumberOfMethodParameters())
                {
                    stringOfDLLMethodParams += ", ";
                }
            }
            stringOfDLLMethodParams += "}";
            return stringOfDLLMethodParams;
        }

        private string paramListGenerator()
        {
            string paramListAsString = "";
            int i = 0;
            foreach (KeyValuePair<int, ParameterModel> pair in this.getAllParametersInThisMethod())
            {
                i++;
                ParameterModel paramAtHand = pair.Value;

                if (paramAtHand.isParameterComplex())
                {
                    paramListAsString += "string " + paramAtHand.getParameterName();
                }
                else
                {
                    paramListAsString += paramAtHand.getTypeOfParameter() + " " + paramAtHand.getParameterName();
                }

                if (i != this.getNumberOfMethodParameters())
                {
                    paramListAsString += ", ";
                }
            }
            return paramListAsString;
        }

        public XmlReader generateXml()
        {
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.Indent = true;

            XmlWriter methodWriter = XmlWriter.Create(stream, writerSettings);
            methodWriter.WriteStartDocument();

            methodWriter.WriteStartElement("method");
            methodWriter.WriteElementString("methodName", this.getMethodName());
            methodWriter.WriteElementString("methodReturnType", this.getMethodReturnType() + "");
            methodWriter.WriteElementString("numberOfParameters", this.getNumberOfMethodParameters() + "");

            methodWriter.WriteStartElement("parameters");

            foreach (KeyValuePair<int, ParameterModel> pair in this.getAllParametersInThisMethod())
            {
                ParameterModel paramAtHand = pair.Value;
                methodWriter.WriteNode(paramAtHand.generateXml(), false);
            }

            methodWriter.WriteEndElement();
            methodWriter.WriteEndElement();
            methodWriter.WriteEndDocument();

            methodWriter.Close();

            stream.Position = 0;
            XmlReader xmlReader = XmlReader.Create(stream);

            return xmlReader;
        }
    }
}
