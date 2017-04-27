using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UIFontDynamicSelect : MonoBehaviour
{
#if UNITY_EDITOR
	[HideInInspector][SerializeField] UIFont m_TargetFont;

	[HideInInspector][SerializeField] Font m_IOSFont;
	[HideInInspector][SerializeField] Font m_WindowPhoneFont;
	[HideInInspector][SerializeField] Font m_AndroidFont;
	[HideInInspector][SerializeField] bool m_AndroidNeedFontFlag;
	[HideInInspector][SerializeField] Font m_OtherFont;

	public UIFont TargetFont
	{
		get { return m_TargetFont; }
		set { m_TargetFont = value;}
	}

	public Font IOSFont
	{
		get { return m_IOSFont; }
		set { m_IOSFont = value; }
	}

	public Font WindowPhoneFont
	{
		get { return m_WindowPhoneFont; }
		set { m_WindowPhoneFont = value; }
	}

	public Font AndroidFont
	{
		get { return m_AndroidFont; }
		set { m_AndroidFont = value; }
	}

	public bool AndroidNeedFontFlag
	{
		get { return m_AndroidNeedFontFlag; }
		set { m_AndroidNeedFontFlag = value; }
	}

	public Font OtherFont
	{
		get { return m_OtherFont; }
		set { m_OtherFont = value; }
	}

	public void MarkAsDirty()
	{
		Font fnt = m_OtherFont;
#if UNITY_IPHONE
		fnt = m_IOSFont;
#elif UNITY_WP8
		fnt = m_WindowPhoneFont;
#elif UNITY_ANDROID
		if (m_AndroidNeedFontFlag)
		{
			fnt = m_AndroidFont;
		}
		else
		{
			if (Application.isPlaying)
			{
				fnt = m_OtherFont;
			}
			else
			{
				fnt = null;
			}
		}
#endif
		if (m_TargetFont != null)
		{
			RegisterUndo("Font change", m_TargetFont);
			m_TargetFont.dynamicFont = fnt;
		}
		else
		{
			Debug.LogWarning("Mark as fontSelect failed.TargetFont can not be null.");
		}
	}

	void RegisterUndo(string name, params Object[] objects)
	{
		if (objects != null && objects.Length > 0)
		{
			foreach (Object obj in objects)
			{
				if (obj == null) continue;
				UnityEditor.Undo.RegisterUndo(obj, name);
				UnityEditor.EditorUtility.SetDirty(obj);
			}
		}
		else
		{
			UnityEditor.Undo.RegisterSceneUndo(name);
		}
	}
#endif
}
