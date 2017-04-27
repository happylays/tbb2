using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Common;

namespace LoveDance.Client.Loader
{
	public class UIWndLoader
	{
		static UIWndLoader uiwndloader
		{
			get
			{
				if (s_UIWndLoader == null)
				{
					s_UIWndLoader = new UIWndLoader();
				}

				return s_UIWndLoader;
			}
		}

		static UIWndLoader s_UIWndLoader = null;

		AssetBundleConfig m_WndConfig = new AssetBundleConfig();	// UI配置文件
		AssetBundleConfig m_PrefabConfig = new AssetBundleConfig();	// 动态加载Prefab配置文件
        Dictionary<AssetBundleType, AssetLoader> m_AssetLoaderMap = new Dictionary<AssetBundleType, AssetLoader>();

		public static void InitUIWndLoader(string assetDir, string assetWWWDir, string inAssetDir, string inAssetWWWDir, string netAssetDir)
		{
			for (AssetBundleType arType = AssetBundleType.Texture; arType < AssetBundleType.Max; ++arType)
			{
                AssetLoader textureLoader = new AssetLoader();
				textureLoader.InitLoader(
                    "UIWndLoader_" + arType.ToString(),
					assetWWWDir + arType + "/",
					assetDir + arType + "/",
					inAssetWWWDir + arType + "/",
					inAssetDir + arType + "/",
                    netAssetDir + arType + "/",
					"." + arType.ToString().ToLower());

				uiwndloader.m_AssetLoaderMap.Add(arType, textureLoader);
			}
		}

		public static IEnumerator LoadUIConfig(string assetDir, string inAssetDir, string netAssetDir)
		{
			IEnumerator itor = uiwndloader.m_WndConfig.LoadConfigSync(assetDir + AssetBundleType.Scene + "/", inAssetDir + AssetBundleType.Scene + "/", netAssetDir + AssetBundleType.Scene + "/", "uiconfig");
			while (itor.MoveNext())
			{
				yield return null;
			}

			itor = uiwndloader.m_PrefabConfig.LoadConfigSync(assetDir + AssetBundleType.Scene + "/", inAssetDir + AssetBundleType.Scene + "/", netAssetDir + AssetBundleType.Scene + "/", "uiprefabconfig");
			while (itor.MoveNext())
			{
				yield return null;
			}
		}
                
		public static IEnumerator PrepareUI()
		{
			//加载公共脚本
			IEnumerator itor = PrepareCommonUI();
			while (itor.MoveNext())
			{
				yield return null;
			}

			//加载其他资源
			List<string> textureData = new List<string>();
			textureData.Add("t_gongyong");
			textureData.Add("t_gongyong_a");
			textureData.Add("t_top");
			textureData.Add("t_top_a");

			if (uiwndloader.m_AssetLoaderMap.ContainsKey(AssetBundleType.Texture))
			{
				AssetLoader texLoader = uiwndloader.m_AssetLoaderMap[AssetBundleType.Texture];
				for (int i = 0; i < textureData.Count; i++)
				{
                    itor = texLoader.LoadAssetSync(textureData[i], true, false, DownLoadOrderType.AfterRunning, true);
					while (itor.MoveNext())//等待本地加载完成;
					{
						yield return null;
					}
				}
			}
		}

		/// <summary>
		/// 动态加载Prefab前必须先调用该方法
		/// </summary>
		/// <returns></returns>
		public static IEnumerator PrepareCommonUI()
		{
			if (uiwndloader.m_AssetLoaderMap.ContainsKey(AssetBundleType.Scene))
			{
				string strResName = "ui_common";

                IEnumerator itor = uiwndloader.m_AssetLoaderMap[AssetBundleType.Scene].LoadAssetSync(strResName, true, false, DownLoadOrderType.AfterRunning, true);
                while (itor.MoveNext())
                {
                    yield return null;
                }
			}
			else
			{
				Debug.LogError("UIWndLoader GetTextureAsset error, can not find texture loader.");
			}
		}

		public static IEnumerator LoadMainWndAsync()
		{
			IEnumerator itor = LoadUIWndAsync("gamemain", true);
			while (itor.MoveNext())
			{
				yield return null;
			}
		}

        public static IEnumerator DownLoadUIWndAsync(string uiID)
        {
            IEnumerator itor = uiwndloader.DownLoadWndAsset(uiID, uiwndloader.m_WndConfig);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

		public static IEnumerator LoadUIWndAsync(string uiID, bool isResident)
		{
			IEnumerator itor = uiwndloader.LoadWndAsset(uiID, isResident, uiwndloader.m_WndConfig);
			while (itor.MoveNext())
			{
				yield return null;
			}

			itor = uiwndloader.InstantiateWndAsset(uiID);
			while (itor.MoveNext())
			{
				yield return null;
			}
		}

		public static void ReleaseUIWnd(string uiID)
		{
			uiwndloader.ReleaseWndAsset(uiID, uiwndloader.m_WndConfig);
		}

		public static void ReleaseAllUIWnd()
		{
			uiwndloader.ReleaseAllWndAsset();
		}


		public static Texture GetTexture(string textureName)
		{
			return uiwndloader.GetTextureAsset(textureName);
		}

		public static IEnumerator LoadTexture(string textureName)
		{
			IEnumerator itor = uiwndloader.LoadTextureAsset(textureName);
			while (itor.MoveNext())
			{
				yield return null;
			}
		}

		public static void ReleaseTexture(string textureName)
		{
			uiwndloader.ReleaseTextureAsset(textureName);
		}

		public static void LoadTextureAsync(string textureName, Callback<string> callBack)
		{
			uiwndloader.LoadTextureAssetAsync(textureName, callBack);
		}

		public static void ReleaseTexture(string textureName, Callback<string> callBack)
		{
			uiwndloader.ReleaseTextureAsset(textureName, callBack);
		}

        IEnumerator DownLoadWndAsset(string wndName,AssetBundleConfig config)
        {
            IEnumerator itor = null;

            SortedDictionary<int, List<DependencyAsset>> prefabData = config.GetConfig(wndName);
            if (prefabData != null)
            {
                foreach (KeyValuePair<int, List<DependencyAsset>> kvp in prefabData)
                {
                    foreach (DependencyAsset dasset in kvp.Value)
                    {
                        if (m_AssetLoaderMap.ContainsKey(dasset.AssetType))
                        {
							itor = m_AssetLoaderMap[dasset.AssetType].LoadAssetSync(dasset.AssetName, false, true, DownLoadOrderType.AfterRunning, true);
                            while (itor.MoveNext())
                            {
                                yield return null;
                            }
							m_AssetLoaderMap[dasset.AssetType].ReleaseAsset(dasset.AssetName, null, true);
                        }
                    }
                }
            }
        }

		IEnumerator LoadWndAsset(string wndName, bool bResident, AssetBundleConfig config)
		{
			IEnumerator itor = null;

			SortedDictionary<int, List<DependencyAsset>> prefabData = config.GetConfig(wndName);
			if (prefabData != null)
			{
				foreach (KeyValuePair<int, List<DependencyAsset>> kvp in prefabData)
				{
					foreach (DependencyAsset dasset in kvp.Value)
					{
						if (m_AssetLoaderMap.ContainsKey(dasset.AssetType))
						{
                            itor = m_AssetLoaderMap[dasset.AssetType].LoadAssetSync(dasset.AssetName, bResident, false, DownLoadOrderType.AfterRunning, true);
                            while (itor.MoveNext())
                            {
                                yield return null;
                            }

							//删除美术图集压缩包（临时解决方案）
							if (CommonValue.PhoneOS == Phone_OS.Ios)
							{
								if (dasset.AssetName.Equals(wndName))
								{
									m_AssetLoaderMap[dasset.AssetType].GetMainAsset(dasset.AssetName);
									m_AssetLoaderMap[dasset.AssetType].UnloadAssetBundle(dasset.AssetName);
								}
								else
								{
									if (dasset.AssetType == AssetBundleType.Pre)
									{
										GameObject obj = m_AssetLoaderMap[dasset.AssetType].GetMainAsset(dasset.AssetName) as GameObject;
										Material spriteMat = CommonValue.GetAtlasMatCB(obj);
										if (spriteMat != null)
										{
											string objPartName = dasset.AssetName.Substring(3);

											Material mat = (Material)m_AssetLoaderMap[AssetBundleType.Material].GetMainAsset("m_" + objPartName);
											Texture mainTex = (Texture)m_AssetLoaderMap[AssetBundleType.Texture].GetMainAsset("t_" + objPartName);
											Texture alphaTex = (Texture)m_AssetLoaderMap[AssetBundleType.Texture].GetMainAsset("t_" + objPartName + "_a");
											if (mat != null)
											{
												if (mainTex != null)
												{
													if (alphaTex != null)
													{
														spriteMat = mat;
														spriteMat.SetTexture("_MainTex", mainTex);
														spriteMat.SetTexture("_AlphaTex", alphaTex);

														m_AssetLoaderMap[AssetBundleType.Material].UnloadAssetBundle("m_" + objPartName);
														m_AssetLoaderMap[AssetBundleType.Texture].UnloadAssetBundle("t_" + objPartName);
														m_AssetLoaderMap[AssetBundleType.Texture].UnloadAssetBundle("t_" + objPartName + "_a");
													}
													else
													{
														Debug.LogWarning("==LoadWndAsset==" + objPartName + ",alphaTex" + alphaTex);
													}
												}
												else
												{
													Debug.LogWarning("==LoadWndAsset==" + objPartName + ",mainTex" + mainTex);
												}
											}
											else
											{
												Debug.LogWarning("==LoadWndAsset==" + objPartName + ",mat" + mat);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		void ReleaseWndAsset(string wndName, AssetBundleConfig config)
		{
			for (AssetBundleType arType = AssetBundleType.Texture; arType < AssetBundleType.Max; ++arType)
			{
				SortedDictionary<int, List<DependencyAsset>> textureData = config.GetConfig(wndName, arType);
				if (textureData != null)
				{
					if (m_AssetLoaderMap.ContainsKey(arType))
					{
						foreach (List<DependencyAsset> list in textureData.Values)
						{
							foreach (DependencyAsset dasset in list)
							{
								m_AssetLoaderMap[arType].ReleaseAsset(dasset.AssetName, null, true);
							}
						}
					}
				}
			}
		}

		void ReleaseAllWndAsset()
		{
			foreach (AssetLoader al in m_AssetLoaderMap.Values)
			{
				al.ReleaseAllAsset();
			}
		}

		Texture GetTextureAsset(string textureName)
		{
			if (m_AssetLoaderMap.ContainsKey(AssetBundleType.Texture))
			{
				return (Texture)m_AssetLoaderMap[AssetBundleType.Texture].GetMainAsset(textureName);
			}
			else
			{
				Debug.LogError("UIWndLoader GetTextureAsset error, can not find texture loader.");
			}

			return null;
		}

		IEnumerator LoadTextureAsset(string textureName)
		{
			if (m_AssetLoaderMap.ContainsKey(AssetBundleType.Texture))
			{
                IEnumerator itor = m_AssetLoaderMap[AssetBundleType.Texture].LoadAssetSync(textureName, false, false, DownLoadOrderType.AfterRunning, true);
                while (itor.MoveNext())
                {
                    yield return null;
                }
			}
			else
			{
				Debug.LogError("UIWndLoader LoadTextureAsset error, can not find texture loader.");
			}
		}

		void ReleaseTextureAsset(string textureName)
		{
			if (m_AssetLoaderMap.ContainsKey(AssetBundleType.Texture))
            {
				m_AssetLoaderMap[AssetBundleType.Texture].ReleaseAsset(textureName, null, true);
			}
			else
			{
				Debug.LogError("UIWndLoader ReleaseTextureAsset error, can not find texture loader.");
			}
		}

		void LoadTextureAssetAsync(string textureName, Callback<string> callBack)
		{
			if (m_AssetLoaderMap.ContainsKey(AssetBundleType.Texture))
			{
				m_AssetLoaderMap[AssetBundleType.Texture].LoadAssetAsync(textureName, false, callBack, false, DownLoadOrderType.AfterRunning, true);
			}
			else
			{
				Debug.LogError("UIWndLoader LoadTextureAsync error, can not find texture loader.");
			}
		}

		void ReleaseTextureAsset(string textureName, Callback<string> callBack)
		{
			if (m_AssetLoaderMap.ContainsKey(AssetBundleType.Texture))
			{
				m_AssetLoaderMap[AssetBundleType.Texture].ReleaseAsset(textureName, callBack, true);
			}
			else
			{
				Debug.LogError("UIWndLoader ReleaseTextureAsset error, can not find texture loader.");
			}
		}

		public static Object GetPrefab(string prefabName)
		{
			return uiwndloader.GetPrefabAsset(prefabName);
		}
		
		/// <summary>
		/// 加载Prefab
		/// </summary>
		/// <param name="prefabName">大小写和源文件保持一致</param>
		public static IEnumerator LoadUIPrefabSync(string prefabName, bool isResident)
		{
			IEnumerator itor = uiwndloader.LoadWndAsset(prefabName, isResident, uiwndloader.m_PrefabConfig);
			while (itor.MoveNext())
			{
				yield return null;
			}
		}
		
		public static void ReleasePrefab(string prefabName)
		{
			uiwndloader.ReleaseWndAsset(prefabName, uiwndloader.m_PrefabConfig);
		}
		
		IEnumerator InstantiateWndAsset(string wndName)
		{
			if (m_AssetLoaderMap.ContainsKey(AssetBundleType.Pre))
			{
				Object obj = m_AssetLoaderMap[AssetBundleType.Pre].GetMainAsset(wndName);
				if (obj != null)
				{
					GameObject instObj = GameObject.Instantiate(obj) as GameObject;
					if (instObj == null)
					{
						Debug.LogError("UIWndLoader error, LoadUISceneSync failed. Instantiate can not be null." + wndName);
					}
					else
					{
						instObj.name = wndName;
						
						Camera[] camArr = instObj.GetComponentsInChildren<Camera>();
						for (int i = 0; i < camArr.Length; ++i)
						{
							Camera cam = camArr[i];
							if (cam != null)
							{
								cam.enabled = false;
							}
						}

						yield return null;	//等待一帧，让实例化出来的UI的Start方法执行完成

						for (int i = 0; i < camArr.Length; ++i)
						{
							Camera cam = camArr[i];
							if (cam != null)
							{
								cam.enabled = true;
							}
						}
					}
				}
				else
				{
					Debug.LogError("UIWndLoader error, LoadUISceneSync failed. UI can not be null." + wndName);
				}
			}
			else
			{
				Debug.LogError("UIWndLoader error, LoadUISceneSync failed. Not contain PrefabAssetType." + wndName);
			}
		}
		
		Object GetPrefabAsset(string prefabName)
		{
			if (m_AssetLoaderMap.ContainsKey(AssetBundleType.Pre))
			{
				return m_AssetLoaderMap[AssetBundleType.Pre].GetMainAsset(prefabName);
			}
			else
			{
				Debug.LogError("UIWndLoader GetPrefabAsset error, can not find texture loader.");
			}

			return null;
		}
	}
}