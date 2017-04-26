using UnityEngine;
using System.Collections;

public class cSceneLoader : cSingleton<cSceneLoader>
{

    public void LoadLevel(string level)
    {
        Application.LoadLevel(level);
    }
}
