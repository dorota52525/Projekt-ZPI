using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ZpiProject.Helpers
{
    public class CamelCase : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var res = base.CreateProperty(member, memberSerialization);

            var attrs = member.GetCustomAttributes(typeof(JsonPropertyAttribute), true);

            if (attrs.Any())
            {
                var attr = (attrs.First() as JsonPropertyAttribute);
                if (res.PropertyName != null)
                    res.PropertyName = attr.PropertyName;
            }

            return res;
        }
    }
}
