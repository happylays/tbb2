using UnityEngine;
using System.Collections;
using System.IO;
using LoveDance.Client.Common.Messengers;
using LoveDance.Client.Common;

namespace LoveDance.Client.Loader
{
	public class ShaderLoader
	{
		static AssetBundle m_ShaderAsset;
		static Object[] m_AllShader;
        
        public static IEnumerator LoadAllShader(string assetDir, string assetWWWDir, string inAssetDir, string inAssetWWWDir,string assetNetDir)
        {
            string strName = "all.shd";
            if (WWWDownLoaderConfig.CheckResNeedUpdate(strName))
            {
				DownLoadPack downloadPack = WWWDownLoader.InsertDownLoad(strName, assetNetDir + strName, DownLoadAssetType.AssetBundle, null, null, DownLoadOrderType.AfterRunning);
				if (downloadPack != null)
				{
					while (!downloadPack.AssetReady)
					{
						yield return null;
					}
					m_ShaderAsset = downloadPack.Bundle;
				}
                WWWDownLoader.RemoveDownLoad(strName, null);
            }
            else
            {
                IEnumerator itor = LoadAllShader(assetDir,  assetWWWDir,  inAssetDir,  inAssetWWWDir);
                while(itor.MoveNext())
                {
                    yield return null;
                }
            }

            if (m_ShaderAsset != null)
            {
                m_AllShader = m_ShaderAsset.LoadAll();
            }
            Shader.WarmupAllShaders();
            Messenger.Broadcast(MessangerEventDef.LOAD_ONEASSET_FINISH, MessengerMode.DONT_REQUIRE_LISTENER);
        }

		private static IEnumerator LoadAllShader(string assetDir, string assetWWWDir, string inAssetDir, string inAssetWWWDir)
		{
			string assetWWWPath = assetWWWDir + "all.shd";
			string assetPath = assetDir + "all.shd";
			if (!File.Exists(assetPath))
			{
				assetWWWPath = inAssetWWWDir + "all.shd";
			}

			WWW www = null;
			using (www = new WWW(assetWWWPath))
			{
				while (!www.isDone)
				{
					yield return null;
				}

				m_ShaderAsset = www.assetBundle;

				www.Dispose();
				www = null;
			}
		}

		public static void ReleaseAllShader()
		{
			if (m_ShaderAsset != null)
			{
				m_AllShader = null;

				AssetBundle shaderBundle = m_ShaderAsset;
				m_ShaderAsset = null;

				shaderBundle.Unload(true);
				shaderBundle = null;
			}
		}

		public static Object[] GetAllShader()
		{
			return m_AllShader;
		}
	}
}