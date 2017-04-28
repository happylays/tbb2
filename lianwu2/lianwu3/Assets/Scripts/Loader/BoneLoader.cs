using UnityEngine;
using System.Collections;
using LoveDance.Client.Common;

namespace LoveDance.Client.Loader
{
	public class BoneLoader
	{
		static BoneLoader boneloader
		{
			get
			{
				if (s_BoneLoader == null)
				{
					s_BoneLoader = new BoneLoader();
				}

				return s_BoneLoader;
			}
		}

		static BoneLoader s_BoneLoader = null;

        AssetLoader m_PlayerBoneLoader = new AssetLoader();

		/// <summary>
		/// 初始化路径
		/// </summary>
        public static void InitPlayerBone(string assetDir, string assetWWWDir, string inAssetDir, string inAssetWWWDir, string assetNetDir)
        {
			boneloader.m_PlayerBoneLoader.InitLoader("BoneLoader",assetWWWDir, assetDir, inAssetWWWDir, inAssetDir, assetNetDir, ".res");
		}
        
		public static IEnumerator LoadPlayerBone()
		{
			IEnumerator itor = boneloader.LoadBone();
			while (itor.MoveNext())
			{
				yield return null;
			}
		}

		public static void RelaeseAllBone()
		{
			boneloader.ReleaseBone();
		}

		public static Object GetPlayerBone()
		{
			return boneloader.GetBone();
		}

		IEnumerator LoadBone()
		{
            IEnumerator itor = m_PlayerBoneLoader.LoadAssetSync("bones", false, false, DownLoadOrderType.AfterRunning, true);
			while (itor.MoveNext())
			{
				yield return null;
			}
		}

		void ReleaseBone()
		{
			m_PlayerBoneLoader.ReleaseAllAsset();
		}

		Object GetBone()
		{
			Object bone = m_PlayerBoneLoader.GetMainAsset("bones");

			if (CommonValue.PhoneOS == Phone_OS.Ios)
			{
				m_PlayerBoneLoader.UnloadAssetBundle("bones");
			}

			return bone;
		}
	}
}