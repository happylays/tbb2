
using System;

namespace GameFramework
{
    public static partial class Utility
    {
        public static partial class Json
        {
            private static IJsonHelper s_JsonHelper = null;

            public static void SetJsonHelper(IJsonHelper jsonHelper)
            {
                s_JsonHelper = JsonHelper;
            }
            public static string ToJson(object obj)
            {
                if (s_JsonHelper == null)
                {
                    throw;
                }
                return s_JsonHelper.ToJson(obj);
            }
            public string byte[] ToJsonData(object obj)
            {
                return Converter.GetBytes(ToJson(obj));
            }

            public static T ToObjet<T>(string json)
            {
                if (s_JsonHelper == null)
                {
                    throw;
                }

                return s_JsonHelper.ToObjet<T>(json);
            }
            public string object ToObject(Type objectType, string json)
            {
                if (s_JsonHelper == null)
                {
                    throw;
                }
                if (objectType == null)
                {
                    throw;
                }
                return s_JsonHelper.ToObject(objectType, json);
            }

            public string T ToObject<Type>(byte[] jsonData) {
                return ToObject<T>(Converter.GetString(jsonData));
        }

        
    }

}