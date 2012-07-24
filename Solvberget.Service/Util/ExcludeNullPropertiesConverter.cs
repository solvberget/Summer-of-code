using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using Solvberget.Domain.DTO;

namespace Solvberget.Service.Util
{

    public class ExcludeNullPropertiesConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var jsonExample = new Dictionary<string, object>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var value = prop.GetValue(obj, BindingFlags.Public, null, null, null);
                if (ValidateProperty(value))
                    jsonExample.Add(prop.Name, value);
            }

            return jsonExample;
        }

        private static bool ValidateProperty(object value)
        {
            // Uncommented beacause the MetroApp expects some fields to exist
            // (Fields that are an empty string)
            //if (value is string)
            //{
            //    return !string.IsNullOrEmpty((string)value);
            //}
            if (value is IList)
            {
                return ((IList)value).Count > 0;
            }

            return value != null;
        
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return typeof(Document).Assembly.GetTypes(); }
        }
    }

}