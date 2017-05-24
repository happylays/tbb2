using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UIFontDynamicSelect))]
public class UIFontDynamicSelectInspector : Editor
{
	UIFontDynamicSelect mFontDynamicSelect;

	override public void OnInspectorGUI()
	{
		mFontDynamicSelect = target as UIFontDynamicSelect;

		EditorGUILayout.LabelField("This script only used in dynamic font.");
		EditorGUILayout.LabelField("This script must be topped to other components.");

		NGUIEditorTools.DrawSeparator();
		UIFont targetFont = EditorGUILayout.ObjectField("Target Font", mFontDynamicSelect.TargetFont, typeof(UIFont), false) as UIFont;
		mFontDynamicSelect.TargetFont = targetFont;

		NGUIEditorTools.DrawSeparator();
		Font iosFont = EditorGUILayout.ObjectField("IOS Font", mFontDynamicSelect.IOSFont, typeof(Font), false) as Font;
		mFontDynamicSelect.IOSFont = iosFont;

		NGUIEditorTools.DrawSeparator();
		Font wpFont = EditorGUILayout.ObjectField("WindowPhone Font", mFontDynamicSelect.WindowPhoneFont, typeof(Font), false) as Font;
		mFontDynamicSelect.WindowPhoneFont = wpFont;

		NGUIEditorTools.DrawSeparator();
		Font androidFont = EditorGUILayout.ObjectField("AndroidFont Font", mFontDynamicSelect.AndroidFont, typeof(Font), false) as Font;
		GUILayout.BeginHorizontal();
		bool isAndroidNeed = EditorGUILayout.Toggle("NeedFont", mFontDynamicSelect.AndroidNeedFontFlag, GUILayout.Width(200f));
		GUILayout.Label("- true:need use TTF file.");
		GUILayout.EndHorizontal();
		mFontDynamicSelect.AndroidFont = androidFont;
		mFontDynamicSelect.AndroidNeedFontFlag = isAndroidNeed;

		NGUIEditorTools.DrawSeparator();
		Font otherFont = EditorGUILayout.ObjectField("Other Font", mFontDynamicSelect.OtherFont, typeof(Font), false) as Font;
		mFontDynamicSelect.OtherFont = otherFont;

		mFontDynamicSelect.MarkAsDirty();
	}
}
