using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CoreFramework.Utils
{
    class JsonUtil
    {
        public static IDictionary<string, object> convertJsonToObject(string jsonAtHand) {
            ExpandoObject jsonAsExpando = JsonConvert.DeserializeObject<ExpandoObject>(
                                               jsonAtHand, new ExpandoObjectConverter());
            IDictionary<string, object> propertyValues = (IDictionary<string, object>) jsonAsExpando;
            return propertyValues;
        }

        public static IDictionary<string, object>  fetchAsDictFromExpando(ExpandoObject expando)
        {
            IDictionary<string, object> propertyValues = (IDictionary<string, object>) expando;
            return propertyValues;
        }
    }
}
