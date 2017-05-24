using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;
using LoveDance.Client.Common.Messengers;


public class SwitchingControl : MonoBehaviour
{
    [SerializeField] GameObject m_CameraObject = null;
	[SerializeField] UITexture m_ADTex = null;
    [SerializeField] float m_nHideSpeedUpTime = 0;
    [SerializeField] float m_nHideWaittingDelayTime = 0;
    [SerializeField] UI_ProgressBar m_UIProgress = null;

	static SwitchingControl s_SwitchingControl = null;
	public delegate void WaitSwitchingEnd();

	int m_nInUsing = 0;
	int m_nADIndex = -1;		//当前图片索引,默认0为本地图片索引
	List<string> m_lstADTex = new List<string>();
	private WaitSwitchingEnd m_WaitSwitchEnd;
	private bool m_IsSwitch = false;
	private string m_DefaultTexture = "splash";	//默认图
	private Vector3 m_OriTexScale;	//记录原始loading图大小
	
	void Awake()
	{
		s_SwitchingControl = this;
		DontDestroyOnLoad( gameObject );
        m_CameraObject.SetActive(false);

		//添加默认图片
		m_lstADTex.Add(m_DefaultTexture);
		m_OriTexScale = m_ADTex.cachedTransform.localScale;

        InitProgressBar();

        Messenger.AddListener(MessangerEventDef.LOAD_ONEASSET_FINISH, OnLoadSomeOneAssetFinish);
	}
	
	void OnDestroy()
	{
		m_lstADTex.Clear();
		s_SwitchingControl = null;

        Messenger.RemoveListener(MessangerEventDef.LOAD_ONEASSET_FINISH, OnLoadSomeOneAssetFinish);
	}

    public static IEnumerator ShowSwitching()
    {
        IEnumerator itor = ShowSwitching(false, 0);
        while(itor.MoveNext())
        {
            yield return null;
        }
    }

    /// <summary>
    /// 显示切换场景loading;
    /// </summary>
    /// <param name="isShowProgress">是否显示进度条</param>
	public static IEnumerator ShowSwitching(bool isShowProgress,int nPercentCount)
	{
		if (s_SwitchingControl != null)
        {
            s_SwitchingControl.SetPercentCount(nPercentCount);

            IEnumerator itor = s_SwitchingControl.ShowOne(isShowProgress);
			while (itor.MoveNext())
			{
				yield return null;
			}
		}
	}

	public static void HideSwitching()
	{
		if (s_SwitchingControl != null)
		{
            s_SwitchingControl.SetTargetPercent(100);
            s_SwitchingControl.StartHideOne();
		}
	}

	public static void AttachWaitEnd(WaitSwitchingEnd waitEnd)
	{
		if (s_SwitchingControl != null)
		{
			if (s_SwitchingControl.m_IsSwitch)
			{
				s_SwitchingControl.m_WaitSwitchEnd = waitEnd;
			}
			else
			{
				waitEnd();
			}
		}
	}
        
    /// <summary>
    /// 设置进度条进度;
    /// </summary>
    /// <param name="nPct">进度,max == 100</param>
    public static void SetSwitchingTargetPercent(int nPct)
    {
        if (s_SwitchingControl != null)
        {
            s_SwitchingControl.SetTargetPercent(nPct);
        }
    }

    /// <summary>
    /// 每个资源下载完后的事件;
    /// </summary>
    private void OnLoadSomeOneAssetFinish()
    {
        if (m_IsSwitch)
        {
            MoveNextPercent();//进度加到下一段;
        }
    }

    void SetPercentCount(int nPct)
    {
        if (m_UIProgress != null)
        {
            m_UIProgress.SetPercentCount(nPct);
        }
    }

    void MoveNextPercent()
    {
        if (m_UIProgress != null)
        {
            m_UIProgress.MoveNextPercent();
        }
    }

    void SetTargetPercent(int nPct)
    {
        if (m_UIProgress != null)
        {
            m_UIProgress.SetTargetPercent(nPct);
        }
    }

    void InitProgressBar()
    {
        if (m_UIProgress != null)
        {
            m_UIProgress.InitProgressBar(m_nHideSpeedUpTime);
        }
    }

    void ShowProgressBar()
    {
        if (m_UIProgress != null)
        {
            m_UIProgress.ResetPercent();
            m_UIProgress.gameObject.SetActive(true);
        }
    }

    void HideProgressBar()
    {
        if (m_UIProgress != null)
        {
            m_UIProgress.gameObject.SetActive(false);
        }
    }

    IEnumerator ShowOne(bool isShowProgress)
    {
        if (isShowProgress)
        {
            ShowProgressBar();
        }

		m_IsSwitch = true;
		if ( m_nInUsing > 0 )
		{
			++m_nInUsing;
		}
		else
		{
            m_CameraObject.SetActive(true);
			m_nInUsing = 1;

			IEnumerator itor = ChangeAd();
			while ( itor.MoveNext() )
			{
				yield return null;
			}
		}
	}

    void StartHideOne()
    {
        StartCoroutine(HideOne());
    }

    IEnumerator HideOne()
	{
        yield return new WaitForSeconds(m_nHideSpeedUpTime + m_nHideWaittingDelayTime);

        HideProgressBar();

		if ( m_nInUsing > 0 )
		{
			--m_nInUsing;
		}

		if ( m_nInUsing <= 0 && gameObject.activeSelf )
		{
            m_CameraObject.SetActive(false);
		}
		m_IsSwitch = false;
		if (m_WaitSwitchEnd != null)
		{
			m_WaitSwitchEnd();
			m_WaitSwitchEnd = null;
		}
	}

	IEnumerator ChangeAd()
	{
		if ( m_lstADTex.Count <= 1 )
		{
            List<string> loadingList = new List<string>();
            loadingList.Add("loading_ad-01");
			int loadingADCount = loadingList.Count;
			for (int i = 0; i < loadingADCount; ++i)
			{
				string loadingad = loadingList[i];
				string adPath = CommonValue.UITextureDir + loadingad + "." + AssetBundleType.Texture.ToString().ToLower();
				if (!File.Exists(adPath))
				{
					adPath = CommonValue.InUITextureDir + loadingad + "." + AssetBundleType.Texture.ToString().ToLower();
				}

                if (File.Exists(adPath) || WWWDownLoaderConfig.CheckResNeedUpdate(loadingad + "." + AssetBundleType.Texture.ToString().ToLower()))
                {
                    m_lstADTex.Add(loadingad);
                }
			}
		}

		int adIndex = 0;
		if ( m_lstADTex.Count > 1 )
		{
			adIndex = Random.Range( 1, m_lstADTex.Count );	// 索引从1开始，表示只关心服务器上数据
		}

		if (m_nADIndex != adIndex)
		{
			string adOld = "";
			if (m_nADIndex >= 0 && m_nADIndex < m_lstADTex.Count)
			{
				adOld = m_lstADTex[m_nADIndex];
			}

			m_nADIndex = adIndex;
			string adNew = m_lstADTex[m_nADIndex];

			IEnumerator itor = ShowLoadingTexture(adNew, adOld);
			while (itor.MoveNext())
			{
				yield return null;
			}
		}
	}

	/// <summary>
	/// 显示loading图
	/// </summary>
	IEnumerator ShowLoadingTexture(string textureName, string oldTextureName)
	{
		if(string.IsNullOrEmpty(textureName))
		{
			Debug.LogError("SwitchingControl ShowLoadingTexture failed, textureName can not be null.");
		}
		else
		{
            m_ADTex.gameObject.SetActive(false);
			IEnumerator itor = ExtraLoader.LoadExtraTextureSync(textureName);
			while (itor.MoveNext())
			{
				yield return null;
			}

			Texture tex = ExtraLoader.GetExtraTexture(textureName);
			if (tex == null)
			{
				m_ADTex.mainTexture = null;
				Debug.LogError("SwitchingControl ShowLoadingTexture failed, texture can not be null." + textureName);
			}
			else
			{
				m_ADTex.mainTexture = tex;
				if (m_DefaultTexture.Equals(textureName))
				{
					m_ADTex.cachedTransform.localScale = CommonFunc.GetTextureScale(tex);	//防止图片变形，因为是1024 * 1024
				}
				else
				{
					m_ADTex.cachedTransform.localScale = m_OriTexScale;
				}
				m_ADTex.gameObject.SetActive(true);
			}

			ExtraLoader.ReleaseExtraTexture(oldTextureName, null);
		}
	}
}
