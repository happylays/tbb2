using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PlayerBone : MonoBehaviour
{
	[SerializeField] Vector3 m_TitlePos = Vector3.zero;
	[SerializeField] Vector3 m_WingOffsetPos = Vector3.zero;
	[SerializeField] Vector3 m_HipOffsetPos = Vector3.zero;
	[SerializeField] Vector3 m_WingOffsetRot = Vector3.zero;
	[SerializeField] Vector3 m_HipOffsetRot = Vector3.zero;
	[SerializeField] Vector3 m_LeftHandOffsetPos = Vector3.zero;//Hands Hold
	[SerializeField] Vector3 m_RightHandOffsetPos = Vector3.zero;
	[SerializeField] Vector3 m_LeftHandOffsetRot = Vector3.zero;
	[SerializeField] Vector3 m_RightHandOffsetRot = Vector3.zero;
	
	[SerializeField] Vector3[] m_ShoulderPos = null;
	[SerializeField] Vector3[] m_ShoulderRot = null;
    [SerializeField] Vector3 m_HeadWearOffsetPos = Vector3.zero;
    [SerializeField] Vector3 m_HeadWearOffsetRot = Vector3.zero;
    [SerializeField] Vector3 m_PantOffsetPos = Vector3.zero;
    [SerializeField] Vector3 m_PantOffestRot = Vector3.zero;
    [SerializeField] Vector3 m_CoatOffsetPos = Vector3.zero;
    [SerializeField] Vector3 m_CoatOffsetRot = Vector3.zero;
	[SerializeField] Vector3 m_WristOffsetPos = Vector3.zero;
	[SerializeField] Vector3 m_WristOffsetRot = Vector3.zero;
	[SerializeField] Vector3 m_CalfLeftOffsetPos = Vector3.zero;
	[SerializeField] Vector3 m_CalfLeftOffsetRot = Vector3.zero;
	[SerializeField] Vector3 m_CalfRightOffsetPos = Vector3.zero;
	[SerializeField] Vector3 m_CalfRightOffsetRot = Vector3.zero;

	public Vector3 TitlePos
	{
		get
		{
			return m_TitlePos;
		}
	}
	
	public Vector3 WingOffsetPos
	{
		get
		{
			return m_WingOffsetPos;
		}
	}
	
	public Vector3 HipOffsetPos
	{
		get
		{
			return m_HipOffsetPos;
		}
	}
	
	public Vector3 WingOffsetRot
	{
		get
		{
			return m_WingOffsetRot;
		}
	}
	
	public Vector3 HipOffsetRot
	{
		get
		{
			return m_HipOffsetRot;
		}
	}
	
	public Vector3 LeftHandOffsetPos {
		get
		{
			return m_LeftHandOffsetPos;
		}
	}
	
	public Vector3 RightHandOffsetPos {
		get
		{
			return m_RightHandOffsetPos;
		}
	}
	
	public Vector3 LeftHandOffsetRot {
		get
		{
			return m_LeftHandOffsetRot;
		}
	}
	
	public Vector3 RightHandOffsetRot {
		get
		{
			return m_RightHandOffsetRot;
		}
	}
	
	public Vector3[] ShoulderOffsetPos {
		get
		{
			return m_ShoulderPos;
		}
	}
	
	public Vector3[] ShoulderOffsetRot {
		get
		{
			return m_ShoulderRot;
		}
	}

    public Vector3 HeadWearOffsetPos
    {
        get { return m_HeadWearOffsetPos; }
    }

    public Vector3 HeadWearOffsetRot
    {
        get { return m_HeadWearOffsetRot; }
    }

    public Vector3 PantOffsetPos
    {
        get { return m_PantOffsetPos; }
    }

    public Vector3 PantOffsetRot
    {
        get { return m_PantOffestRot; }
    }

    public Vector3 CoatOffsetPos
    {
        get { return m_CoatOffsetPos; }
    }

    public Vector3 CoatOffsetRot
    {
        get { return m_CoatOffsetRot; }
    }

	public Vector3 WristOffsetPos
	{
		get
		{ return m_WristOffsetPos; }
	}

	public Vector3 WristOffsetRot
	{
		get { return m_WristOffsetRot; }
	}
	public Vector3 CalfLeftOffsetPos 
	{
		get { return m_CalfLeftOffsetPos; }
	}
	public Vector3 CalfLeftOffsetRot
	{
		get { return m_CalfLeftOffsetRot; }
	}
	public Vector3 CalfRightOffsetPos
	{
		get { return m_CalfRightOffsetPos; }
	}
	public Vector3 CalfRightOffsetRot
	{
		get { return m_CalfRightOffsetRot; }
	}

	public Transform m_LeftHandBone;
	public Transform m_RightHandBone;
	public Transform m_RootBone;
	
	public Transform m_BackBone;
	public Transform m_TailBone;
	
	public Transform[] m_ShoulderBone;

    public Transform m_HeadWear;
    public Transform m_Coat;
    public Transform m_Pant;
	public Transform m_Wrist;
	public Transform m_CalfLeft;
	public Transform m_CalfRight;
	
#if UNITY_EDITOR
	void Update()
	{
		if( !Application.isPlaying )
		{
			m_WingOffsetPos = m_BackBone.position;
			m_WingOffsetRot = m_BackBone.rotation.eulerAngles;
			
			m_HipOffsetPos = m_TailBone.position;
			m_HipOffsetRot = m_TailBone.rotation.eulerAngles;
			
			m_LeftHandOffsetPos = m_LeftHandBone.position;
			m_LeftHandOffsetRot = m_LeftHandBone.rotation.eulerAngles;
	
			m_RightHandOffsetPos = m_RightHandBone.position;
			m_RightHandOffsetRot = m_RightHandBone.rotation.eulerAngles;
			
			if( m_ShoulderPos.Length != m_ShoulderBone.Length )
			{
				m_ShoulderPos = new Vector3[m_ShoulderBone.Length];
			}

			if( m_ShoulderRot.Length != m_ShoulderBone.Length )
			{
				m_ShoulderRot = new Vector3[m_ShoulderBone.Length];
			}

			for( int i =0 ;i<m_ShoulderBone.Length;i++ )
			{
				m_ShoulderPos[i] = m_ShoulderBone[i].position;
				m_ShoulderRot[i] = m_ShoulderBone[i].rotation.eulerAngles;
			}

            m_HeadWearOffsetPos = m_HeadWear.position;
            m_HeadWearOffsetRot = m_HeadWear.rotation.eulerAngles;

            m_PantOffsetPos = m_Pant.position;
            m_PantOffestRot = m_Pant.rotation.eulerAngles;

            m_CoatOffsetPos = m_Coat.position;
            m_CoatOffsetRot = m_Coat.rotation.eulerAngles;

			if(m_Wrist != null)
			{
				m_WristOffsetPos = m_Wrist.position;
				m_WristOffsetRot = m_Wrist.rotation.eulerAngles;
			}

			m_CalfLeftOffsetPos = m_CalfLeft.position;
			m_CalfLeftOffsetRot = m_CalfLeft.rotation.eulerAngles;
			m_CalfRightOffsetPos = m_CalfRight.position;
			m_CalfRightOffsetRot = m_CalfRight.rotation.eulerAngles;
		}
	}
#endif
}
