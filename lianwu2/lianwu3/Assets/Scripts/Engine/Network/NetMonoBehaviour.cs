using UnityEngine;
using LoveDance.Client.Network;

public class NetMonoBehaviour : MonoBehaviour
{
	NetMsgObserver m_NetObserver = new NetMsgObserver();

	public NetMsgObserver NetObserver
	{
		get
		{
			return m_NetObserver;
		}
	}

	protected virtual void OnEnable()
	{
		m_NetObserver.Enable();
	}

	protected virtual void OnDisable()
	{
		m_NetObserver.Disable();
	}

	protected virtual void OnDestroy()
	{
		m_NetObserver.ClearNetMsgProcessor();
		m_NetObserver = null;
	}
}

