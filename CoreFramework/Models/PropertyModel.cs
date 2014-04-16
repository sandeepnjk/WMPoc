using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CoreFramework.Models
{
    [Serializable()]
    public class PropertyModel : ISerializable
    {
        private string propertyName;
        private Type propertyType;
        private bool isPropertyComplex;

        protected PropertyModel(SerializationInfo info, StreamingContext ctx)
        {
            this.propertyName = (string)info.GetValue("propertyName", typeof(string));
            this.propertyType = (Type)info.GetValue("propertyType", typeof(Type));
        }

        public PropertyModel(string propertyName, Type propertyType)
        {
            this.propertyName = propertyName;
            this.propertyType = propertyType;
        }

        public string getPropertyName()
        {
            return this.propertyName;
        }

        public Type getPropertyType()
        {
            return this.propertyType;
        }

        public bool isComplexProperty()
        {
            return this.isPropertyComplex;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("propertyName", this.propertyName);
            info.AddValue("propertyType", this.propertyType);
        }
    }
}
