using UnityEngine;

/// <summary>
/// 该脚本用于开启Coroutine,开启的Coroutine是DontDestroyOnLoad.
/// 不用于开启UI。
/// </summary>
public class MainCoroutine : MonoBehaviour
{
    private static MainCoroutine mInstance = null;

    public static MainCoroutine mainCoroutine
    {
        get
        {
            return mInstance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        mInstance = this;
    }

    void OnDestroy()
    {
        mInstance = null;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public static void InitMainCoroutine()
    {
        GameObject go = new GameObject();
        if (go != null)
        {
            go.name = "_MainCoroutine";
            go.AddComponent<MainCoroutine>();
        }
        else
        {
            Debug.LogError("Init MainCoroutine faild. GameObjet can not be null.");
        }
    }
}