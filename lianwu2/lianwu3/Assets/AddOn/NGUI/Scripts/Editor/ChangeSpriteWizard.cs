using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class ChangeSpriteWizard : EditorWindow
{
	bool mLogDetailFlag = false;	//获取Sprite名时，切换Log打印
	string mAtlasFilters = "pr_texture_1,pr_texture_2,pr_texture_3,pr_texture_4";		//检查图集过滤器

	void OnGUI()
	{
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		GUI.color = Color.green;
		GUILayout.Label("1. Replace simple sprite to slice sprite and double scale.");
		GUI.color = Color.white;
		bool spriteBtn = GUILayout.Button("Replace", GUILayout.Width(300f));
		if (spriteBtn)
		{
			Object[] list = Resources.FindObjectsOfTypeAll(typeof(UISprite));
			foreach (Object obj in list)
			{
				if (obj is UISprite && string.IsNullOrEmpty(AssetDatabase.GetAssetPath(obj)))
				{
					UISprite mSprite = obj as UISprite;
					Vector3 oriScale = mSprite.transform.localScale;
					UIAtlas.Sprite oriSprite = mSprite.GetAtlasSprite();
					if (mSprite != null && oriSprite != null && mSprite.type == UISprite.Type.Simple)
					{
						if (oriScale.x <= oriSprite.outer.width || oriScale.x + 2 <= oriSprite.outer.width)
						{
							mSprite.transform.localScale *= 2;
						}

						mSprite.type = UISprite.Type.Sliced;
						NGUIEditorTools.RegisterUndo("Sprite Change", mSprite);
						EditorUtility.SetDirty(mSprite.gameObject);
					}
				}
			}
		}

		SeparateLine();

		GUI.color = Color.green;
		GUILayout.Label("2. Get the information of atlas in sprite.");
		GUI.color = Color.white;
		bool checkSpriteName = GUILayout.Button("Sprite Atlas Information", GUILayout.Width(300f));
		mLogDetailFlag = GUILayout.Toggle(mLogDetailFlag, "Detail Log", GUILayout.Width(100f));
		if (checkSpriteName)
		{
			UISprite[] list = Resources.FindObjectsOfTypeAll(typeof(UISprite)) as UISprite[];
			foreach (UISprite obj in list)
			{
				if (mLogDetailFlag)
				{
					if (obj.atlas != null)
					{
						Debug.LogException(new System.Exception("=AtlasName=" + obj.name + "," + obj.atlas + "," + obj.spriteName), obj);
					}
					else
					{
						Debug.LogException(new System.Exception("null atlas=" + obj.name), obj);
					}
				}
				else
				{
					if (obj.atlas != null)
					{
						Debug.Log("AtlasName=" + obj.atlas);
					}
					else
					{
						Debug.Log("null atlas=" + obj.name);
					}
				}
			}
		}

		SeparateLine();

		GUI.color = Color.green;
		GUILayout.Label("3. 检查当前打开场景的图集.");
		GUI.color = Color.white;
		GUILayout.Label("当前检查的场景：" + EditorApplication.currentScene);
		GUILayout.BeginHorizontal();
		GUILayout.Label("图鉴过滤器：", GUILayout.Width(60f));
		if (string.IsNullOrEmpty(PlayerPrefs.GetString("CheckingAtlasFilter")))
		{
			PlayerPrefs.SetString("CheckingAtlasFilter", mAtlasFilters);
		}
		mAtlasFilters = GUILayout.TextField(PlayerPrefs.GetString("CheckingAtlasFilter"));
		PlayerPrefs.SetString("CheckingAtlasFilter", mAtlasFilters);
		GUILayout.EndHorizontal();
		bool checkAtlas = GUILayout.Button("检 查", GUILayout.Width(300f));
		if (checkAtlas)
		{
			string[] arr = mAtlasFilters.Split(',');
			List<string> filterList = new List<string>(arr);

			UISprite[] list = Resources.FindObjectsOfTypeAll(typeof(UISprite)) as UISprite[];
			if (list.Length > 0)
			{
				foreach (UISprite obj in list)
				{
					if (obj != null && obj.atlas != null && filterList.Contains(obj.atlas.name))
					{
						Debug.LogException(new System.Exception("Font Name: " + obj.transform.root + "," + obj.atlas.name), obj.gameObject);
					}
				}
			}
			else
			{
				Debug.Log("No same atlas find.");
			}
		}

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
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
