using System.Text;
using Newtonsoft.Json;
using System;

namespace NETCoreRemoveServices.Core.Reflection
{
    public static class Serializer
    {
        public static byte[] SerializeObject(object obj)
        {
            string val = JsonConvert.SerializeObject(obj);
            return Encoding.ASCII.GetBytes(val);
        }

        public static T DeserializeObject<T>(byte[] bytes)
        {
            string obj = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            return DeserializeObject<T>(obj);
        }

        public static T DeserializeObject<T>(object obj)
        {
            return JsonConvert.DeserializeObject<T>(obj.ToString());
        }


        public static object DeserializeObject(object instance, Type type)
        {
            //TODO validate instance is JObject \ valid string
            return JsonConvert.DeserializeObject(instance.ToString(), type);
        }
    }
}