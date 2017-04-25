using UnityEngine;
using System.Collections;

public class cSingleton<T> where T : new() {

    private static T mInstance = default(T);

    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new T();
            }

            //if (mInstance == null) 
            //{
            //    GameObject obj = new GameObject("cSingleton");
            //    mInstance = obj.AddComponent(typeof(T)) as T;
            //}

            return mInstance;
        }
    }

    void OnApplicationQuit()
    {
        mInstance = default(T);
    }
}
