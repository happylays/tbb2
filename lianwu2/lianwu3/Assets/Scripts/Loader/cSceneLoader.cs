using UnityEngine;
using System.Collections;
using LoveDance.Client.Common;
using LoveDance.Client.Loader;
using LoveDance.Client.Common.Messengers;

public class SceneLoader
{
    static private AssetLoader m_sceneAssetLoader = new AssetLoader();
    static private bool m_isLoading = false;

    public static void InitSceneLoader(string sceneWWWDir, string sceneDir, string inSceneWWWDir, string inSceneDir, string netDir)
    {
        m_sceneAssetLoader.InitLoader("SceneLoader", sceneWWWDir, sceneDir, inSceneWWWDir, inSceneDir, netDir, ".scene");
    }

    /// <summary>
    /// for下载;
    /// </summary>
    public static IEnumerator DownLoadStageScene(string stageName)
    {
        IEnumerator itor = m_sceneAssetLoader.LoadAssetSync(stageName, false, true, DownLoadOrderType.AfterRunning, true);
        while (itor.MoveNext())
        {
            yield return null;
        }
        m_sceneAssetLoader.ReleaseAsset(stageName, null, true);
    }

    public static IEnumerator LoadStageScene(string stageName)
    {
        IEnumerator itor = LoadStageScene(stageName, false);
        while (itor.MoveNext())
        {
            yield return null;
        }
    }

    public static IEnumerator LoadStageScene(string stageName, bool bAdditive)
    {
        if (!m_isLoading)
        {//防止重复加载;
            m_isLoading = true;

            Messenger.Broadcast(MessangerEventDef.StartLoadStage, MessengerMode.DONT_REQUIRE_LISTENER);
            yield return null;	//Waiting two frame
            yield return null;

            IEnumerator itor = LoadEmpty();
            while (itor.MoveNext())
            {
                yield return null;
            }

            if (CommonValue.ClearFontCB != null)
            {
                CommonValue.ClearFontCB();//Clear dynamicFont cache
            }

            AsyncOperation async = Resources.UnloadUnusedAssets();
            while (!async.isDone)
            {
                yield return null;
            }

            itor = m_sceneAssetLoader.LoadAssetSync(stageName, false, false, DownLoadOrderType.AfterRunning, true);
            while (itor.MoveNext())
            {
                yield return null;
            }

            AssetBundle sceneBundle = m_sceneAssetLoader.GetMainAssetBundle(stageName);
            if (sceneBundle != null)
            {
                AsyncOperation asyncOp = null;
                if (bAdditive)
                {
                    asyncOp = Application.LoadLevelAdditiveAsync(stageName);
                }
                else
                {
                    asyncOp = Application.LoadLevelAsync(stageName);
                }

                if (asyncOp != null)
                {
                    while (!asyncOp.isDone)
                    {
                        yield return null;
                    }
                }
                else
                {
                    Debug.LogError("scene level load error: " + stageName);
                }

                m_sceneAssetLoader.UnloadAssetBundle(stageName);
                m_sceneAssetLoader.ReleaseAsset(stageName, null, true);
            }

            m_isLoading = false;
        }
    }

    static IEnumerator LoadEmpty()
    {
        AsyncOperation asyncOp = Application.LoadLevelAsync("EmptyScene");
        if (asyncOp != null)
        {
            while (!asyncOp.isDone)
            {
                yield return null;
            }
        }
        else
        {
            Debug.LogError("empty scene load error");
        }
    }

}
