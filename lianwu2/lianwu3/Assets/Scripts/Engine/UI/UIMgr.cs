/****************************************************************
 * 
 * 
 *			修改人： 蔡辉杰
 *			修改时间：2016年918国耻日
 *			修改内容：
 *					1、所以可以切换到其他wnd的uiwnd 需要继承UIMenuWnd
 *					2、所有UIMenuWnd，关闭时调用closeui，并且需要重写OnRefreshWnd刷新界面
 *					3、有且只能存在2个UIMenuWnd的界面，打开第三个时不管第二个是否关闭，都会关闭最开始的那个wnd
 *					4、切换SCENE时，需要充值MainUI，调用SetMainUI接口
 ****************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;
using LoveDance.Client.Network;
using LoveDance.Client.Logic;

public class UIMgr
{
	static UIMgr uimgr
	{
		get
		{
			if( s_UIMgr == null )
			{
				s_UIMgr = new UIMgr();
			}

			return s_UIMgr;
		}
	}
	
	static UIMgr s_UIMgr = null;
	
	Dictionary<UIFlag, UIWnd> m_mapAllUIWnd = new Dictionary<UIFlag, UIWnd>();
	List<UIFlag> m_lstCacheUIID = new List<UIFlag>();
	List<UIFlag> m_lstShowUIID = new List<UIFlag>();
	private UIFlag m_MainWnd = UIFlag.none;

    public static void Init() { }

	private bool IsShowing(UIFlag flag)
	{
		for (int i = 0; i < m_lstShowUIID.Count;++i )
		{
			if (m_lstShowUIID[i] == flag)
				return true;
		}

		return false;
	}

	public static void RegisterUI(UIWnd uiWnd)
	{
		uimgr.RegisterWnd( uiWnd );
	}

	public static void ShowUISync(UIFlag showID, object exData)
	{
		ShowUISync(showID, UIFlag.none, exData, UIFlag.none);
	}

	public static void ShowUISync(UIFlag showID, object exData,UIFlag hideID)
	{
		ShowUISync(showID, UIFlag.none, exData, hideID);
	}

	public static void ShowUISync(UIFlag showID, UIFlag preID, object exData)
	{
		ShowUISync(showID, preID, exData, UIFlag.none);
	}

	public static void ShowUISync(UIFlag showID, UIFlag preID, object exData, UIFlag hideID)
	{
		UICoroutine.uiCoroutine.StartCoroutine(uimgr.ShowWndAsync(showID, preID, exData, hideID));
	}

	public static IEnumerator ShowUIAsync(UIFlag showID, object exData)
	{
		IEnumerator itor = uimgr.ShowWndAsync(showID, UIFlag.none, exData,UIFlag.none);
		while ( itor.MoveNext() )
		{
			yield return null;
		}
	}

	public static IEnumerator ShowUIAsync(UIFlag showID, UIFlag preID,object exData)
	{
		IEnumerator itor = uimgr.ShowWndAsync(showID, preID, exData, UIFlag.none);
		while (itor.MoveNext())
		{
			yield return null;
		}
	}

	public static IEnumerator ShowUIAsync(UIFlag showID, object exData, UIFlag hideID)
	{
		IEnumerator itor = uimgr.ShowWndAsync(showID, UIFlag.none, exData, hideID);
		while (itor.MoveNext())
		{
			yield return null;
		}
	}

	public static IEnumerator ShowUIAsync(UIFlag showID, UIFlag preID, object exData,UIFlag hideID)
	{
		IEnumerator itor = uimgr.ShowWndAsync(showID, preID, exData, hideID);
		while (itor.MoveNext())
		{
			yield return null;
		}
	}

    public static IEnumerator DownLoadUISync(UIFlag showID)
    {
        IEnumerator itor = uimgr.DownLoadWndSync(showID);
        while (itor.MoveNext())
        {
            yield return null;
        }
    }
	
	public static IEnumerator PrepareUIAsync(UIFlag showID)
	{
		IEnumerator itor = uimgr.PrepareWndAsync( showID );
		while ( itor.MoveNext() )
		{
			yield return null;
		}
	}

	public static IEnumerator SwitchUIAsync(UIFlag showID,object exData)
	{
		IEnumerator itor = uimgr.SwitchWndAsync(showID, UIFlag.none, exData);
		while (itor.MoveNext())
		{
			yield return null;
		}
	}

	public static IEnumerator SwitchUIAsync(UIFlag showID, UIFlag preID, object exData)
	{
		IEnumerator itor = uimgr.SwitchWndAsync(showID, preID, exData);
		while (itor.MoveNext())
		{
			yield return null;
		}
	}

	public static void HideUI(UIFlag hideID)
	{
		uimgr.HideWndSync(  hideID );
	}

	public static IEnumerator DestoryAllUIAsync()
	{
        IEnumerator itor =  uimgr.DestoryAllWndAsync();
        while (itor.MoveNext())
        {
            yield return null;
        }
	}

    public static void DestoryAllUI()
    {
        uimgr.DestroyAllWndImmediate();
    }

	public static void SetMainUI(UIFlag uiID)
	{
		uimgr.SetMainWnd(uiID);
	}

	public static void CloseUI(UIWnd uiWnd)
	{
		UICoroutine.uiCoroutine.StartCoroutine(uimgr.CloseWnd(uiWnd));
	}

	public static IEnumerator CloseUIAsync(UIWnd uiWnd)
	{
		IEnumerator itor = uimgr.CloseWnd(uiWnd);
		while (itor.MoveNext())
		{
			yield return null;
		}
	}

	void RegisterWnd(UIWnd uiWnd)
	{
		if ( uiWnd != null && uiWnd.UIID != UIFlag.none )
		{
			AudioListener[] arListener = uiWnd.GetComponentsInChildren<AudioListener>();
			int listenerLength = arListener.Length;
			for (int i = 0; i < listenerLength; ++i)
			{
				if (arListener[i] != null)
				{
					GameObject.Destroy(arListener[i]);
				}
			}

			///set ui layer
			CommonFunc.SetLayer( uiWnd.gameObject, GameLayer.UI, true,GameLayer.NONE );

			///set camera mask
			Camera ca = uiWnd.camera;
			if ( ca != null )
			{
				int uiMask = ( 1 << (int)GameLayer.UI );
				ca.cullingMask |= uiMask;

				UICamera uiCa = uiWnd.GetComponent<UICamera>();
				if ( uiCa != null )
				{
					uiCa.eventReceiverMask |= uiMask;
				}
			}

			///set label font
			GlobalFunc.SetFont( uiWnd.gameObject );

            ///Set camera clearFlag
			GlobalFunc.SetCameraClearFlags(uiWnd.gameObject, CameraClearFlags.Depth, CameraClearFlags.Nothing);

            if ( m_mapAllUIWnd.ContainsKey( uiWnd.UIID ) )
			{
				Debug.LogError( "ui already register, please check : " + uiWnd.UIID.ToString() );
			}
			else
			{
				m_mapAllUIWnd.Add( uiWnd.UIID, uiWnd );
				m_lstCacheUIID.Remove( uiWnd.UIID );
			}
		}
	}

	IEnumerator ShowWndAsync(UIFlag showID, UIFlag preID, object exData, UIFlag hideID)
	{
        //AmusementMgr.StopMove();
        //LoadingMgr.ShowLoading(true);

		CheckPreUI(preID, hideID);
		HideWndSync(hideID);
        if (!IsUICaching(showID))
        {
            IEnumerator itor = PrepareWndAsync(showID);
            while (itor.MoveNext())
            {
                yield return null;
            }

			itor = SwitchWndAsync(showID, preID,exData);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        //TipsBoxMgr.HideAllTipsBox();

        //LoadingMgr.ShowLoading(false);
	}

    IEnumerator DownLoadWndSync(UIFlag uiID)
    {
        if (uiID != UIFlag.none)//正确的ID;
        {
            IEnumerator itor = UIWndLoader.DownLoadUIWndAsync(uiID.ToString());
            while (itor.MoveNext())
            {
                yield return null;
            }
        }
    }

	IEnumerator PrepareWndAsync(UIFlag uiID)
	{
		if (uiID != UIFlag.none)
		{
			UIWnd uiWnd = GetUIWnd(uiID);
			if (uiWnd == null)
			{
				if (!IsUICaching(uiID))
				{
					m_lstCacheUIID.Add(uiID);

					IEnumerator itor = UIWndLoader.LoadUIWndAsync(uiID.ToString(), false);
					while (itor.MoveNext())
					{
						yield return null;
					}

					uiWnd = GetUIWnd(uiID);
					if (uiWnd != null)
					{
						uiWnd.gameObject.SetActive(false);
					}
					else
					{
						Debug.LogError("prepare ui failed: " + uiID.ToString());
					}
				}
			}
		}
	}

	private void CheckPreUI(UIFlag preID,UIFlag hideID)
	{
		//最多同时显示2个wnd
		UIFlag flag = preID;
		if(flag == UIFlag.none)
		{
			flag = hideID;
		}

		if(flag != UIFlag.none)
		{
			UIWnd preWnd = GetUIWnd(flag);
			if (preWnd != null && preWnd.PreWndID != UIFlag.none)
			{
				if(IsShowing(flag))
				{
					HideWndSync(preWnd.PreWndID);
				}
			}
		}
	}

	IEnumerator SwitchWndAsync(UIFlag targetID, UIFlag preID, object exData)
	{
		if ( targetID != UIFlag.none )
		{		
			if ( !IsUIShowing( targetID ) )
			{
				UIWnd targetWnd = GetUIWnd( targetID );
				if ( targetWnd != null )
				{
					targetWnd.gameObject.SetActive( true );
					m_lstShowUIID.Add(targetID);


					targetWnd.ReadyShow = false;

					WndData targetData = new WndData(exData);
					targetWnd.PreWndID = preID;
					targetWnd.OnShowWnd( targetData );

					while (!targetWnd.ReadyShow)
					{
						yield return null;
					}
				}
				else
				{
					Debug.LogError( "ui should be prepared before switched: " + targetID.ToString() );
				}
			}
		}
	}

	IEnumerator CloseWnd(UIWnd uiWnd)
	{
		if (uiWnd != null)
		{
			IEnumerator itor = null;
			HideWndSync(uiWnd.UIID);

			///LoadingMgr.ShowLoading(true);
			if (uiWnd.PreWndID != UIFlag.none)
			{
				if (!IsShowing(uiWnd.PreWndID))
				{
					UIWnd preWnd = GetUIWnd(uiWnd.PreWndID);
					if (preWnd != null)
					{
						m_lstShowUIID.Add(preWnd.UIID);
						preWnd.ReadyShow = false;

						preWnd.gameObject.SetActive(true);
						preWnd.OnRefreshWnd();

						while (!preWnd.ReadyShow)
						{
							yield return null;
						}
					}
				}
			}
			else
			{
				if(m_lstShowUIID.Count == 0)
				{
					itor = ShowWndAsync(m_MainWnd, UIFlag.none, null, UIFlag.none);
					while (itor.MoveNext())
					{
						yield return null;
					}
				}
			}

			uiWnd.PreWndID = UIFlag.none;

            ////LoadingMgr.ShowLoading(false);
		}
	}

	void HideWndSync(UIFlag hideID)
	{
		if (hideID != UIFlag.none)
		{
			if (IsUIShowing(hideID))
			{
				m_lstShowUIID.Remove(hideID);

				UIWnd hideWnd = GetUIWnd(hideID);
				if (hideWnd != null)
				{
					hideWnd.OnHideWnd();
					hideWnd.gameObject.SetActive(false);
				}
			}
		}
	}

/*	void DestoryWndSync(UIFlag uiID)
	{
		if ( _IsUIShowing( uiID ) )
		{
			m_lstShowUIID.Remove( uiID );

			UIWnd uiWnd = _GetUIWnd( uiID );
			if ( uiWnd != null )
			{
				uiWnd.OnHideWnd();
				uiWnd.gameObject.SetActive( false );

				GameObject.Destroy( uiWnd.gameObject );
				UIWndLoader.ReleaseUIWnd( uiID );

				m_mapAllUIWnd.Remove( uiID );
			}
		}
	}*/

    void DestroyAllWndImmediate()
    {
        UICoroutine.uiCoroutine.gameObject.SetActive(false);	//停止所有UICoroutine
        UICoroutine.uiCoroutine.gameObject.SetActive(true);

        int showIDCount = m_lstShowUIID.Count;
        for (int i = 0; i < showIDCount; ++i)
        {
            UIFlag hideID = m_lstShowUIID[i];
            UIWnd hideWnd = GetUIWnd(hideID);
            if (hideWnd != null)
            {
                hideWnd.OnHideWnd();
                hideWnd.gameObject.SetActive(false);
            }
        }

        m_lstShowUIID.Clear();

        Dictionary<UIFlag, UIWnd> mapAllMap = new Dictionary<UIFlag, UIWnd>(m_mapAllUIWnd);
        m_mapAllUIWnd.Clear();
        m_lstCacheUIID.Clear();

        foreach (KeyValuePair<UIFlag, UIWnd> kvp in mapAllMap)
        {
            if (kvp.Value != null && kvp.Value.gameObject)
            {
                GameObject.DestroyImmediate(kvp.Value.gameObject);
            }
            UIWndLoader.ReleaseUIWnd(kvp.Key.ToString());
        }
        mapAllMap.Clear();

        ////DynamicPrefabMgr.Instance.OnDestroyUI();
        UIWndLoader.ReleaseAllUIWnd();		//check again
        UIAtlasLoader.ReleaseAllAtlas();
        //TipsBoxMgr.HideAllTipsBox();
        //MsgWaitMgr.StopWaiting();

        //LoadingMgr.HideViolently();
    }

	IEnumerator DestoryAllWndAsync()
	{
		UICoroutine.uiCoroutine.gameObject.SetActive(false);	//停止所有UICoroutine
		UICoroutine.uiCoroutine.gameObject.SetActive(true);

		int showIDCount = m_lstShowUIID.Count;
		for (int i = 0; i < showIDCount; ++i)
		{
			UIFlag hideID = m_lstShowUIID[i];
			UIWnd hideWnd = GetUIWnd( hideID );
			if ( hideWnd != null )
			{
				hideWnd.OnHideWnd();
				hideWnd.gameObject.SetActive( false );
			}
		}

		m_lstShowUIID.Clear();

		Dictionary<UIFlag, UIWnd> mapAllMap = new Dictionary<UIFlag, UIWnd>(m_mapAllUIWnd);
		m_mapAllUIWnd.Clear();
		m_lstCacheUIID.Clear();
		
		foreach ( KeyValuePair<UIFlag, UIWnd> kvp in mapAllMap )
		{
			if (kvp.Value != null && kvp.Value.gameObject)
			{
				GameObject.Destroy(kvp.Value.gameObject);

                yield return null; //wait for a frame
			}
			UIWndLoader.ReleaseUIWnd( kvp.Key.ToString() );
		}
		mapAllMap.Clear();

        ////DynamicPrefabMgr.Instance.OnDestroyUI();
		UIWndLoader.ReleaseAllUIWnd();		//check again
		UIAtlasLoader.ReleaseAllAtlas();
        //TipsBoxMgr.HideAllTipsBox();
        //MsgWaitMgr.StopWaiting();

        //LoadingMgr.HideViolently();
	}

	void SetMainWnd(UIFlag uiID)
	{
		m_MainWnd = uiID;
	}

	public static UIWnd GetUIWnd(UIFlag uiID)
	{
		if ( uiID != UIFlag.none )
		{
			if ( uimgr.m_mapAllUIWnd.ContainsKey( uiID ) )
			{
				return uimgr.m_mapAllUIWnd[uiID];
			}
		}

		return null;
	}

	public static bool IsUICaching(UIFlag uiID)
	{
		if ( uiID != UIFlag.none && uimgr.m_lstCacheUIID.Contains(uiID))
		{
			return true;
		}

		return false;
	}

	public static bool IsUIShowing(UIFlag uiID)
	{
		if (uiID != UIFlag.none && uimgr.IsShowing(uiID))
		{
			return true;
		}
		
		return false;
	}

}
