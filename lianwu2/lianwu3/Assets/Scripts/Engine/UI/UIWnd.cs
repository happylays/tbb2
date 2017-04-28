using UnityEngine;
using System.Collections.Generic;
using System;
using LoveDance.Client.Common;

public class WndData
{
	public object ExData
	{
		get
		{
			return m_ExData;
		}
	}

	object m_ExData = null;

	public WndData( object exData)
	{
		m_ExData = exData;
	}
}


public abstract class UIWnd : NetMonoBehaviour
{
	public abstract UIFlag UIID
	{
		get;
	}

	public bool ReadyShow
	{
		get
		{
			return m_bReadyShow;
		}
		set
		{
			m_bReadyShow = value;
		}
	}

	private bool m_bReadyShow = false;
	private List<Delegate> eventList = null;
	private UIFlag m_PreWndID = UIFlag.none;

	public UIFlag PreWndID
	{
		get
		{
			return m_PreWndID;
		}
		set
		{
			m_PreWndID = value;
		}
	}

	public virtual UIType UIWndType
	{
		get
		{
			return UIType.Normal;
		}
	}

	public virtual void Awake()
	{
		UIMgr.RegisterUI( this );
	}

	public virtual void OnShowWnd(WndData wndData)
	{
		m_bReadyShow = true;
	}

	public virtual void OnHideWnd()
	{
		InvokeHideWndHanlder();
	}

	public virtual void OnRefreshWnd()
	{
		m_bReadyShow = true;
	}

	public void AddHideWndHanlder(Callback del)
	{
		if (eventList == null)
		{
			eventList = new List<Delegate>();
		}

		eventList.Add(del);
	}

	void InvokeHideWndHanlder()
	{
		if (eventList != null)
		{
			int eventListCount = eventList.Count;
			for (int i = 0; i < eventListCount; ++i)
			{
				Callback cb = eventList[i] as Callback;
				cb();
			}
		}
	}
}

public abstract class UIMenuWnd : UIWnd
{
	public override UIType UIWndType
	{
		get
		{
			return UIType.Menu;
		}
	}
}
