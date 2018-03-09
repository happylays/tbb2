
using GameFramework;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    internal class JsonHelper : Utility.Json.IJsonHelper
    {
        public string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public T ToObject<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
        public object ToObject(Type objectType, string json)
        {
            return JsonUtility.FromJson(json, objectType);
        }
    }

}