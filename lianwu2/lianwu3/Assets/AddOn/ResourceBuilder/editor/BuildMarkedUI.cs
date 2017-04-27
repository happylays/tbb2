/******************************************************************************
					Copyright (C), 2014-2015, DDianle Tech. Co., Ltd.
					Name:BuildMarkedUI.cs
					Author: Caihuijie
					Description: 
					CreateDate: 2015.07.09
					Modify: 
******************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
using LoveDance.Client.Common;

public class BuildMarkedUI : EditorWindow
{
	class AssetItem
	{
		private bool mIsChecked = false;
		private string mAssetName = null;

		public AssetItem() { }

		public bool IsChecked { get { return mIsChecked; } set { mIsChecked = value; } }
		public string AssetName { get { return mAssetName; } set { mAssetName = value; } }
	}

	private const string UIScenePrefs = "MarkedUISceneList";
	private const string UIPrefs = "MarkedUI";
	private const string TexturePrefs = "MarkedTexture";
	
	List<AssetItem> m_listEnum = new List<AssetItem>();
	List<string> m_MarkedUIList = new List<string>();

	private bool m_HasGetUIScene = false;
	private Vector2 mScrollPosition = Vector2.zero;
	private Vector2 mChooseScrollPosition = Vector2.zero;

	private Rect windowRect = new Rect(0, 0, 500, 700);

	private Vector2 windowSize = new Vector2(510, 720);

	private bool m_ChooseUI = false;
    private bool m_TempChooseAllUI = false;
    private bool m_ChooseAllUI = false;
	private bool m_ChooseTexture = false;
	private string m_strTexture = string.Empty;

	void OnGUI()
	{
		BeginWindows();
		minSize = windowSize;
		maxSize = windowSize;
		windowRect = GUILayout.Window(1, windowRect, AllWindow, "");        
		EndWindows();
	}

	void AllWindow(int id)
	{
		ShowChooseTab();

		if (!m_HasGetUIScene)
		{
			GetAllUIScene();
			m_HasGetUIScene = true;
		}

		ShwoAllUIScene();

		GUI.color = Color.green;
		if (GUILayout.Button("开始编译"))
		{
			StartMarkedUI();
		}

		DrawSeparator();
		ShowAllChooseUI();
	}

	void ShowChooseTab()
	{
		m_ChooseTexture = GUILayout.Toggle(m_ChooseTexture, "Texture");
		GUI.color = Color.green;
		EditorGUILayout.LabelField("编译指定的Texture，不填则全部编译。");
		GUI.color = Color.white;
		m_strTexture = EditorGUILayout.TextField(m_strTexture);

		m_ChooseUI = GUILayout.Toggle(m_ChooseUI, "UI");
		GUI.color = Color.green;
		GUILayout.Label("请选择需要编译的UI");
		GUI.color = Color.white;
	}

    void SetUIState(bool state)
    {
        foreach (AssetItem item in m_listEnum)
        {
            item.IsChecked = state;
        }
    }

	void ShwoAllUIScene()
	{
        GUI.color = Color.green;
        if (GUILayout.Button("加载配置"))
        {
            string filename = EditorUtility.OpenFilePanel("选择配置文件", "", "txt");
            if (!string.IsNullOrEmpty(filename))
            {
                LoadMarkedUI(filename);
            }     
        }
        if (GUILayout.Button("保存配置"))
        {
            string filename = EditorUtility.SaveFilePanel("保存配置文件", "","", "txt");
            if (!string.IsNullOrEmpty(filename))
            {
                SaveMarkedUI(filename);
            } 
        }
        GUI.color = Color.white;

        m_TempChooseAllUI = GUILayout.Toggle(m_TempChooseAllUI, "全选/反选");    

        if (m_TempChooseAllUI != m_ChooseAllUI)
        {
            if (m_TempChooseAllUI)
            {
                SetUIState(true);
            }
            else
            {
                SetUIState(false);
            }
            m_ChooseAllUI = m_TempChooseAllUI;
        }

		mScrollPosition = GUILayout.BeginScrollView(mScrollPosition, GUILayout.Width(500), GUILayout.Height(300));

		foreach (AssetItem item in m_listEnum)
		{
			item.IsChecked = GUILayout.Toggle(item.IsChecked, item.AssetName);
		}

		GUILayout.EndScrollView();
	}

	void ShowAllChooseUI()
	{
		GUI.color = Color.red;
		GUILayout.Label("已经勾选的UI");

		mChooseScrollPosition = GUILayout.BeginScrollView(mChooseScrollPosition, GUILayout.Width(500), GUILayout.Height(200));
		foreach (AssetItem item in m_listEnum)
		{
			if (item.IsChecked)
			{
                GUILayout.BeginHorizontal();
                GUI.color = Color.yellow;
				GUILayout.Label(item.AssetName);

                GUI.color = Color.white;
                if (GUILayout.Button("del", GUILayout.Width(46f)))
                {
                    item.IsChecked = false;
                }
                GUILayout.EndHorizontal();
			}
		}
		GUI.color = Color.white;
		GUILayout.EndScrollView();
	}

	void GetAllUIScene()
	{
		string uiscene = PlayerPrefs.GetString(UIScenePrefs);
		string[] uisceneArray = uiscene.Split('|');

		for (int i = 0; i < uisceneArray.Length; ++i)
		{
			if (!string.IsNullOrEmpty(uisceneArray[i]))
			{
				m_MarkedUIList.Add(uisceneArray[i]);
			}
		}

		List<string> uisceneList = BuildAssetBundle.GetAllUIPrefab();

		for (int i = 0; i < uisceneList.Count; ++i)
		{
			AssetItem item = new AssetItem();
			item.IsChecked = HasMarkedUI(uisceneList[i]);
			item.AssetName = uisceneList[i];
			m_listEnum.Add(item);
		}

		m_ChooseUI = PlayerPrefs.GetInt(UIPrefs) == 1 ? true : false;
		m_ChooseTexture = PlayerPrefs.GetInt(TexturePrefs) == 1 ? true : false;
	}

	void StartMarkedUI()
	{
		List<string> uiScenePrefabList = new List<string>();
		StringBuilder sb = new StringBuilder();


		foreach (AssetItem item in m_listEnum)
		{
			if (item.IsChecked)
			{
				if (m_ChooseUI)
				{
					uiScenePrefabList.Add(item.AssetName);
				}
				sb.Append(item.AssetName);
				sb.Append("|");
			}
		}

		PlayerPrefs.DeleteKey(UIScenePrefs);
		PlayerPrefs.DeleteKey(UIPrefs);
		PlayerPrefs.DeleteKey(TexturePrefs);

		PlayerPrefs.SetString(UIScenePrefs, sb.ToString());

		PlayerPrefs.SetInt(UIPrefs, m_ChooseUI ? 1: 0);
		PlayerPrefs.SetInt(TexturePrefs, m_ChooseTexture ? 1 : 0);

		List<string> specialList = new List<string>();
		List<string> prefabList = new List<string>();

		if (m_ChooseTexture)
		{
			List<string> uiTxtSrcDirList = new List<string>();

			uiTxtSrcDirList.Add(AssetBundlePath.SpecialTexBasicAssetDir);
#if !PACKAGE_BASIC
			uiTxtSrcDirList.Add(AssetBundlePath.SpecialTexAssetDir);
#endif

			foreach (string specialDir in uiTxtSrcDirList)
			{
				if (Directory.Exists(specialDir))
				{
					BuildAssetList(specialDir, specialList, AssetBundleType.Texture,m_strTexture);
				}
				else
				{
					Debug.Log("path is not exist: " + specialDir);
				}
			}
		}

		List<string> uiPrefabSrcDirList = new List<string>();
		uiPrefabSrcDirList.Add(AssetBundlePath.PrefabBasicAssetDir);
#if !PACKAGE_BASIC
		uiPrefabSrcDirList.Add(AssetBundlePath.PrefabAssetDir);
#endif
		foreach (string uiPrefabDir in uiPrefabSrcDirList)
		{
			if (Directory.Exists(uiPrefabDir))
			{
				BuildAssetList(uiPrefabDir, prefabList, AssetBundleType.Pre,string.Empty);
			}
			else
			{
				Debug.Log("path is not exist: " + uiPrefabDir);
			}
		}

		BuildUI.ProcUIScene(specialList, uiScenePrefabList, prefabList);
	}

	bool HasMarkedUI(string scene)
	{
		for (int i = 0; i < m_MarkedUIList.Count; ++i)
		{
			if (m_MarkedUIList[i].Equals(scene))
				return true;
		}

		return false;
	}

	void DrawSeparator()
	{
		GUILayout.Space(12f);

		if (Event.current.type == EventType.Repaint)
		{
			Texture2D tex = EditorGUIUtility.whiteTexture;
			Rect rect = GUILayoutUtility.GetLastRect();
			GUI.color = new Color(0f, 0f, 0f, 0.25f);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
			GUI.color = Color.white;
		}
	}

	/// <summary>
	/// 获取指定资源
	/// </summary>
	static void BuildAssetList(string dirPath, List<string> fileList, AssetBundleType assetType,string filterAsset)
	{
        string[] filters = filterAsset.Split(';');

		string[] fileArr = Directory.GetFiles(dirPath);
		foreach (string filePath in fileArr)
		{
			if (BuildAssetBundle.GetAssetType(filePath) == assetType)
			{
				if (string.IsNullOrEmpty(filterAsset))
				{
					fileList.Add(filePath);
				}
				else
				{
                    foreach (string filter in filters)
                    {
                        if (filePath.Contains(filter))
                        {
                            fileList.Add(filePath);
                            break;
                        }
                    }
				}
			}
		}

		string[] dirArr = Directory.GetDirectories(dirPath);
		foreach (string dir in dirArr)
		{
			if (!dir.Contains("/."))
			{
				BuildAssetList(dir, fileList, assetType,filterAsset);
			}
		}
	}

    void LoadMarkedUI(string filePath)
    {
        if (File.Exists(filePath))
        {
            List<string> filePathList = new List<string>();
            using (StreamReader sr = new StreamReader(filePath, CommonFunc.GetCharsetEncoding()))
            {
                string strLine = null;
                while ((strLine = sr.ReadLine()) != null)
                {
                    filePathList.Add(strLine);
                }
                sr.Close();
            }
            
            if(filePathList.Count > 0)
            {
                SetUIState(false);

                foreach (AssetItem item in m_listEnum)
                {
                    if (filePathList.Contains(item.AssetName))
                    {
                        item.IsChecked = true;
                    }
                }
            }
        }
    }

    void SaveMarkedUI(string filePath)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        using (StreamWriter sw = new StreamWriter(filePath, false, CommonFunc.GetCharsetEncoding()))
        {
            foreach (AssetItem item in m_listEnum)
            {
                if(item.IsChecked)
                {
                    sw.WriteLine(item.AssetName);
                }
            }
            sw.Close();
        }
    }
}