using UnityEngine;
using System.Collections;
using LoveDance.Client.Common;

namespace LoveDance.Client.Loader
{
    public class ExtraLoader
    {
        static ExtraLoader extraloader
        {
            get
            {
                if (s_ExtraLoader == null)
                {
                    s_ExtraLoader = new ExtraLoader();
                }

                return s_ExtraLoader;
            }
        }

        static ExtraLoader s_ExtraLoader = null;

        AssetLoader m_ExtraTexLoader = new AssetLoader();

        public static void InitExtraLoader(string assetDir, string assetWWWDir, string inAssetDir, string inAssetWWWDir, string assetNetDir)
        {
            extraloader.m_ExtraTexLoader.InitLoader("ExtraLoader", assetWWWDir, assetDir, inAssetWWWDir, inAssetDir, assetNetDir, "." + AssetBundleType.Texture.ToString().ToLower());
        }

        public static Texture GetExtraTexture(string textureName)
        {
            return extraloader.GetExtraTextureAsset(textureName);
        }

        /// <summary>
        /// 对外接口 同步加载or下载图片;
        /// </summary>
        /// <param name="textureName">图片名;</param>
        public static IEnumerator LoadExtraTextureSync(string textureName)
        {
            IEnumerator itor = extraloader.LoadExtraTextureAssetSync(textureName);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        /// <summary>
        /// 对外接口 异步加载or下载图片;
        /// </summary>
        /// <param name="textureName">图片名;</param>
        /// <param name="callBack">下载完成后,回调函数;</param>
        public static void LoadExtraTextureAsync(string textureName, Callback<string> callBack)
        {
            extraloader.LoadExtraTextureAssetAsync(textureName, callBack);
        }

        /// <summary>
        /// 卸载资源;
        /// </summary>
        public static void ReleaseExtraTexture(string textureName, Callback<string> callBack)
        {
            extraloader.ReleaseExtraTextureAsset(textureName, callBack);
        }

        /// <summary>
        /// 提取一个LoadingTexture资源;
        /// </summary>
        Texture GetExtraTextureAsset(string textureName)
        {
            Texture t = (Texture)m_ExtraTexLoader.GetMainAsset(textureName);

            if (CommonValue.PhoneOS == Phone_OS.Ios)
            {
                m_ExtraTexLoader.UnloadAssetBundle(textureName);
            }

            return t;
        }

        /// <summary>
        /// 异步 下载or加载 资源;
        /// </summary>
        void LoadExtraTextureAssetAsync(string textureName, Callback<string> callBack)
        {
            m_ExtraTexLoader.LoadAssetAsync(textureName, false, callBack, false, DownLoadOrderType.AfterRunning, true);
        }

        /// <summary>
        /// 同步 下载or加载 资源;
        /// </summary>
        IEnumerator LoadExtraTextureAssetSync(string textureName)
        {
            IEnumerator itor = m_ExtraTexLoader.LoadAssetSync(textureName, false, false, DownLoadOrderType.AfterRunning, true);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        /// <summary>
        /// 释放资源;
        /// </summary>
        void ReleaseExtraTextureAsset(string textureName, Callback<string> callBack)
        {
            m_ExtraTexLoader.ReleaseAsset(textureName, callBack, true);
        }
    }
}