using UnityEngine;

/// <summary>
/// 仅用于开启UI界面Coroutine。
/// 该脚本对象会在UIMgr.DestroyAllUI()里面停止所有该脚本开启的Coroutine。
/// </summary>
public class UICoroutine : MonoBehaviour 
{
	private static UICoroutine mInstance = null;

	public static UICoroutine uiCoroutine
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
	public static void InitUICoroutine()
	{
		GameObject go = new GameObject();
		if (go != null)
		{
			go.name = "_UICoroutine";
			go.AddComponent<UICoroutine>();
		}
		else
		{
			Debug.LogError("Init UICoroutine faild. GameObjet can not be null.");
		}
	}
}
