using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ChangeLabelFontWizard : EditorWindow
{
	UIFont mOldFont = null;
	UIFont mNewFont = null;
	UIFont mColorFont = null;
	Color mColor = Color.white;

	void OnGUI()
	{
		GUILayout.BeginHorizontal();
		GUI.color = Color.cyan;
		string curScene = EditorApplication.currentScene.Replace('\\', '/');
		curScene = curScene.Substring(curScene.LastIndexOf('/') + 1);
		GUILayout.Label("Current scene: " + curScene, GUILayout.MinWidth(10000f));
		GUI.color = Color.white;
		GUILayout.EndHorizontal();

		SeparateLine();

		GUILayout.BeginHorizontal();
		GUI.color = Color.green;
		GUILayout.Label("1. Change font,this will effect all UILabel,include prefab.", GUILayout.MinWidth(10000f));
		GUI.color = Color.white;
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Select old font", GUILayout.Width(100f));
		ComponentSelector.Draw<UIFont>(mOldFont, OnSelectOldFont, GUILayout.Width(140f));
		GUILayout.EndHorizontal();

		GUILayout.Space(10f);

		GUILayout.BeginHorizontal();
		GUI.color = Color.red;

		if (mOldFont == null)
		{
			GUILayout.Label("Need replace font:  none font", GUILayout.MinWidth(10000f));
		}
		else
		{
			GUILayout.Label("Need replace font:  " + mOldFont.name, GUILayout.MinWidth(10000f));
		}

		GUI.color = Color.white;
		GUILayout.EndHorizontal();

		GUILayout.Space(10f);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Select new font", GUILayout.Width(100f));
		ComponentSelector.Draw<UIFont>(mNewFont, OnSelectNewFont, GUILayout.Width(140f));
		GUILayout.EndHorizontal();

		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		bool retVal = GUILayout.Button("Replace font", GUILayout.Width(150f));
		if (retVal)
		{
			Object[] list = Resources.FindObjectsOfTypeAll(typeof(UILabel));
			foreach (Object obj in list)
			{
				if (obj is UILabel && string.IsNullOrEmpty(AssetDatabase.GetAssetPath(obj)))
				{
					UILabel lbl = obj as UILabel;
					if (lbl != null && lbl.font == mOldFont)
					{
						lbl.font = mNewFont;
						NGUIEditorTools.RegisterUndo("Font Change", lbl.font);
						//Debug.LogError("=11=" + lbl);
					}
				}
			}
		}
		GUILayout.EndHorizontal();

		SeparateLine();

		GUILayout.BeginVertical();
		GUI.color = Color.green;
		GUILayout.Label("2. Color tint all labels will be effect.");
		GUI.color = Color.white;

		GUILayout.BeginHorizontal();
		GUILayout.Label("Select font", GUILayout.Width(100f));
		ComponentSelector.Draw<UIFont>(mColorFont, OnSelectColorFont, GUILayout.Width(140f));
		GUILayout.EndHorizontal();

		GUILayout.Space(5f);

		GUILayout.BeginHorizontal();
		Color c = EditorGUILayout.ColorField("Forground Color", mColor, GUILayout.Width(220f));
		if (c != mColor)
		{
			mColor = c;
		}

		bool colorBtn = GUILayout.Button("Replace color", GUILayout.Width(150f));
		if (colorBtn)
		{
			Object[] list = Resources.FindObjectsOfTypeAll(typeof(UILabel));
			foreach (Object obj in list)
			{
				if (obj is UILabel && string.IsNullOrEmpty(AssetDatabase.GetAssetPath(obj)))
				{
					UILabel lbl = obj as UILabel;
					if (lbl != null && lbl.font == mColorFont)
					{
						lbl.color = c;
						NGUIEditorTools.RegisterUndo("Label Change", lbl);
					}
				}
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		SeparateLine();

		GUILayout.BeginHorizontal();
		GUI.color = Color.green;
		GUILayout.Label("3. Checking label fashionfont,this will effect all UILabel,include prefab.", GUILayout.MinWidth(10000f));
		GUI.color = Color.white;
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		bool checkLabelFont = GUILayout.Button("Checking fashionfont", GUILayout.Width(150f));
		if (checkLabelFont)
		{
			string sceneRootPath = "Assets/Scenes/";
			string[] sceneArr = Directory.GetFiles(sceneRootPath, "*.unity", SearchOption.AllDirectories);
			for(int i = 0;i < sceneArr.Length; ++i)
			{
				string scenePath = sceneArr[i];
				bool openSuc = EditorApplication.OpenScene(scenePath);
				if(openSuc)
				{
					UILabel[] list = Resources.FindObjectsOfTypeAll(typeof(UILabel)) as UILabel[];
					foreach (UILabel obj in list)
					{
						if (obj != null && obj.font != null && obj.font.name.Equals("fashion_font"))
						{
							Debug.LogError("Font Name: " + obj.transform.root + "," + scenePath);
						}
					}
				}
				else
				{
					Debug.LogError("Open scene failed. " + scenePath);
				}
			}
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(30f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("-Editor by naipin", GUILayout.MinWidth(10000f));
		GUILayout.EndHorizontal();
	}

	void OnSelectNewFont(MonoBehaviour obj)
	{
		mNewFont = obj as UIFont;
		Repaint();
	}

	void OnSelectOldFont(MonoBehaviour obj)
	{
		mOldFont = obj as UIFont;
		Repaint();
	}

	void OnSelectColorFont(MonoBehaviour obj)
	{
		mColorFont = obj as UIFont;
		Repaint();
	}

	void SeparateLine()
	{
		GUILayout.BeginHorizontal();
		GUI.color = Color.gray;
		GUILayout.Label("========================================");
		GUI.color = Color.white;
		GUILayout.EndHorizontal();
	}
}