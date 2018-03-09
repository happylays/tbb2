
using System;

namespace GameFramework
{
    public static partial class Utility
    {
        public static partial class Json
        {
            public interface IJsonHelper
            {
                string ToJson(object obj);
                T ToObject<T>(string json);
                object ToObject(Type objectType, string json);
            }
        }
    }
}