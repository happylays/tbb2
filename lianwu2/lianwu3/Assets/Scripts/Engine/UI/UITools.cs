
using UnityEngine;

public class UITools
{
    static public GameObject getGameObject(GameObject gameObj, string path)
    {
        string[] names = path.Split('.');
        Transform trans = gameObj.transform;

        for (int i = 0; i < names.Length; i++)
        {
            if (trans != null)
            {
                trans = trans.FindChild(names[i]);
            }
        }

        GameObject retGameObj = null;
        if (trans != null)
        {
            retGameObj = trans.gameObject;
        }

        return retGameObj;
    }

}