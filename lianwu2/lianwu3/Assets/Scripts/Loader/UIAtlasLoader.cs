using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Common;

namespace LoveDance.Client.Loader
{
	public class UIAtlasLoader
	{
		static UIAtlasLoader uiatlasloader
		{
			get
			{
				if (s_UIAtlasLoader == null)
				{
					s_UIAtlasLoader = new UIAtlasLoader();
				}

				return s_UIAtlasLoader;
			}
		}

		static UIAtlasLoader s_UIAtlasLoader = null;
        private static string m_assetNetDir = "";

        AssetLoader m_AtlasLoader = new AssetLoader();

        public static void InitUIAtlasLoader(string assetDir, string assetWWWDir, string inAssetDir, string inAssetWWWDir, string assetNetDir)
		{
            m_assetNetDir = assetNetDir;
			uiatlasloader.m_AtlasLoader.InitLoader("UIAtlasLoader",assetWWWDir, assetDir, inAssetWWWDir, inAssetDir, assetNetDir,".atl");
		}

        /// <summary>
        /// only for 下载;
        /// </summary>
        public static IEnumerator DownLoadUIAtlasAsync(string atlasName)
        {
            IEnumerator itor = uiatlasloader.DownLoadAtlasAsset(atlasName);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

		public static IEnumerator LoadUIAtlasAsync(string atlasName)
		{
			IEnumerator itor = uiatlasloader.LoadAtlasAsset(atlasName, false);
			while (itor.MoveNext())
			{
				yield return null;
			}
		}

		public static void ReleaseUIAtlas(string atlasName)
		{
			uiatlasloader.ReleaseAtlasAsset(atlasName);
		}

		public static void ReleaseAllAtlas()
		{
			uiatlasloader.ReleaseAllAsset();
		}

		public static Object GetUIAtlas(string atlasName)
		{
			return uiatlasloader.GetAltas(atlasName);
		}

		public static IEnumerator PrepareMotionAtlas(List<string> atlasNames)
		{
			foreach (string atlasName in atlasNames)
			{
				IEnumerator itor = uiatlasloader.LoadAtlasAsset(atlasName, true);
				while (itor.MoveNext())
				{
					yield return null;
				}
			}
		}

        IEnumerator DownLoadAtlasAsset(string atlasName)
        {
            IEnumerator itor = m_AtlasLoader.LoadAssetSync(atlasName, false, true, DownLoadOrderType.AfterRunning, true);
            while (itor.MoveNext())
            {
                yield return null;
            }
			
            m_AtlasLoader.ReleaseAsset(atlasName, null, true);
        }

		IEnumerator LoadAtlasAsset(string atlasName, bool bResident)
		{
            IEnumerator itor = m_AtlasLoader.LoadAssetSync(atlasName, bResident, false, DownLoadOrderType.AfterRunning, true);
            while (itor.MoveNext())
            {
                yield return null;
            }
		}

		void ReleaseAtlasAsset(string atlasName)
        {
			m_AtlasLoader.ReleaseAsset(atlasName, null, true);
		}

		void ReleaseAllAsset()
		{
			m_AtlasLoader.ReleaseAllAsset();
		}

		Object GetAltas(string atlasName)
		{
			Object atlas = (Object)m_AtlasLoader.GetMainAsset(atlasName);

			if (CommonValue.PhoneOS == Phone_OS.Ios)
			{
				m_AtlasLoader.UnloadAssetBundle(atlasName);
			}

			return atlas;
		}

        /// <summary>
        /// 组合完成资源路径名;
        /// </summary>
        private static string MakeUIAtlasPath( string atlasName )
        {
            return m_assetNetDir + atlasName + ".atl";
        } 
	}
}