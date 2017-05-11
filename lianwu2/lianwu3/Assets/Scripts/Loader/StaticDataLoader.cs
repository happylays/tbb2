using UnityEngine;
using System.Collections;
using System.IO;
using LoveDance.Client.Loader;
using LoveDance.Client.Common;
using LoveDance.Client.Network.Music;
using LoveDance.Client.Data.Scene;
using LoveDance.Client.Data;
using LoveDance.Client.Data.Model;


namespace LoveDance.Client.Logic.Ress
{
    public class StaticDataLoader
    {
        public IEnumerator Load(string strFileName)
        {
            byte[] gtBytes = null;
            if (WWWDownLoaderConfig.CheckResNeedUpdate(strFileName))
            {
                DownLoadPack downloadPack = WWWDownLoader.InsertDownLoad(strFileName, CommonValue.NetStaDataDir + strFileName, DownLoadAssetType.Text, null, null, DownLoadOrderType.AfterRunning);
                if (downloadPack != null)
                {
                    while (!downloadPack.AssetReady)
                    {
                        yield return null;
                    }
                    gtBytes = downloadPack.DataBytes;
                }
                WWWDownLoader.RemoveDownLoad(strFileName, null);
            }
            else
            {
                if (!string.IsNullOrEmpty(strFileName))
                {
                    string assetWWWPath = CommonValue.StaDataWWWDir + strFileName;
                    string assetPath = CommonValue.StaDataDir + strFileName;
                    if (!File.Exists(assetPath))
                    {
                        assetWWWPath = CommonValue.InStaDataWWWDir + strFileName;
                        assetPath = CommonValue.InStaDataDir + strFileName;
                    }

                    WWW www = null;
                    using (www = new WWW(assetWWWPath))
                    {
                        while (!www.isDone)
                        {
                            yield return null;
                        }

                        if (www.error != null)
                        {
                            Debug.LogError(www.error);
                            Debug.LogError("StaticData Load Error! AssetName : " + strFileName);
                        }
                        else
                        {
                            gtBytes = www.bytes;
                        }

                        www.Dispose();
                        www = null;
                    }
                }
                else
                {
                    Debug.LogError("StaticData load error, FileName can not be null.");
                }
            }

            if (gtBytes != null)
            {
                LoadDataFromBuffer(gtBytes);
            }
            gtBytes = null;
            //Messenger.Broadcast(MessangerEventDef.LOAD_ONEASSET_FINISH, MessengerMode.DONT_REQUIRE_LISTENER);
        }

        public virtual void LoadDataFromBuffer(byte[] bytes)
        {
        }
    }

    public class CMusicDataLoader : StaticDataLoader
    {
        public override void LoadDataFromBuffer(byte[] bytes)
        {
            XQFileStream file = new XQFileStream();


            if (file != null)
            {
                file.Open(bytes);
            }
            {
                CMusicInfoManager MusicinfoMgr = CMusicInfoManager.MusicDataMgr;
                bool nRes = MusicinfoMgr.LoadMusic(file);

                if (null != MusicinfoMgr)
                {
                    if (!nRes)
                    {
                        Debug.LogError("CMusicInfoManager load data failed!");
                    }

                    nRes = MusicinfoMgr.LoadStage(file);
                }
                else
                {
                    Debug.LogError("CMusicInfoManager is null!");
                }

                CSceneInfoManager ScenceinfoMgr = StaticData.SceneDataMgr;
                if (null != ScenceinfoMgr)
                {
                    nRes = ScenceinfoMgr.Load(file);
                    if (!nRes)
                    {
                        Debug.LogError("CSceneInfoManager load data failed!");
                    }
                }
                else
                {
                    Debug.LogError("CSceneInfoManager is null!");
                }

                CModelInfoManager ModelinfoMgr = StaticData.ModelDataMgr;
                if (null != ModelinfoMgr)
                {
                    nRes = ModelinfoMgr.Load(file);
                    if (!nRes)
                    {
                        Debug.LogError("CModelInfoManager load data failed!");
                    }
                }
                else
                {
                    Debug.LogError("CModelInfoManager is null!");
                }
                file.Close();
            }
        }
    }
}