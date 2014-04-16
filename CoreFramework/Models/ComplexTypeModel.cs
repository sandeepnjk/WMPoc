using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;


namespace CoreFramework.Models
{
    [Serializable()]
    public class ComplexTypeModel : ISerializable
    {
        protected ComplexTypeModel(SerializationInfo info, StreamingContext ctx)  { 
        this.actualTypeName = (string) info.GetValue("actualTypeName", typeof(string));
        this.allFieldsInThisComplexType = (Dictionary<string, FieldModel>)
            info.GetValue("allFieldsInThisComplexType", typeof(Dictionary<string, FieldModel>));
        }
        private Dictionary<string, FieldModel> allFieldsInThisComplexType = new Dictionary<string, FieldModel>();
        private Dictionary<string, PropertyModel> allPropertiesInThisComplexType = new Dictionary<string, PropertyModel>();
        private ConstructorModel constructorModel;
        private string actualTypeName;
        private string dllFileThisTypeBelongsTo;
        private Type representationalTypeFromAssembly;

        public string getActualTypeName()
        {
            return this.actualTypeName;
        }

        public Dictionary<string, FieldModel> getAllFieldsInThisComplexType()
        {
            return this.allFieldsInThisComplexType;
        }

        public ComplexTypeModel(string actualTypeName)
        {
            this.actualTypeName = actualTypeName;
        }

        public void setDllFileThisTypeBelongsTo(string dllFileThisTypeBelongsTo)
        {
            this.dllFileThisTypeBelongsTo = dllFileThisTypeBelongsTo;
        }

        public string getDllFileThisTypeBelongsTo()
        {
            return this.dllFileThisTypeBelongsTo;
        }

        public Dictionary<string, PropertyModel> getAllPropertiesInThisComplexType()
        {
            return this.allPropertiesInThisComplexType;
        }

        public XmlReader generateXml()
        {
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.Indent = true;
            XmlWriter classWriter = XmlWriter.Create(stream, writerSettings);

            classWriter.WriteStartElement("complexType");

            classWriter.WriteElementString("typeName", this.actualTypeName);

            foreach (KeyValuePair<string, FieldModel> pair in this.getAllFieldsInThisComplexType())
            {
                FieldModel fieldAtHand = pair.Value;
                classWriter.WriteNode(fieldAtHand.generateXml(), false);
            }

            classWriter.WriteEndElement();
            classWriter.Flush();
            stream.Position = 0;
            XmlReader xmlReader = XmlReader.Create(stream);
            classWriter.Close();
            return xmlReader;
        }

        public ConstructorModel getConstructorModel()
        {
            return this.constructorModel;
        }

        public void setConstructorModel(ConstructorModel constModel)
        {
            this.constructorModel = constModel;
        }

        public void setRepresentationalTypeFromAssembly(Type representationalTypeFromAssembly)
        {
            this.representationalTypeFromAssembly = representationalTypeFromAssembly;
        }

        public Type getRepresentationalTypeFromAssembly()
        {
            return this.representationalTypeFromAssembly;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("allFieldsInThisComplexType", this.allFieldsInThisComplexType);
            info.AddValue("allPropertiesInThisComplexType", this.allPropertiesInThisComplexType);
            info.AddValue("actualTypeName", this.actualTypeName);
        }

    }


}
