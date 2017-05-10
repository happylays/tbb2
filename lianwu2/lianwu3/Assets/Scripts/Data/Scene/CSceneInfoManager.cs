using System;
using LoveDance.Client.Common;

namespace LoveDance.Client.Data.Scene
{
    public class CSceneInfoManager
    {

        private XQHashtable s_SceneInfoMap = new XQHashtable();

        //scene
        public bool Load()
        {
            DestroyScene();
            XQFileStream file = new XQFileStream();
            if (file != null)
            {
                file.Open("Data/sceneinfo");
                if (file.IsOpen())
                {

                    UInt16 usNumber = 0;
                    file.ReadUShort(ref usNumber);

                    for (UInt16 i = 0; i < usNumber; i++)
                    {
                        CSceneInfo sceneinfo = new CSceneInfo();
                        sceneinfo.Load(ref file);
                        RegistSceneInfo(sceneinfo);
                    }
                    file.Close();
                    s_SceneInfoMap.Sort();
                    return true;
                }
            }
            return false;
        }

        public bool Load(XQFileStream file)
        {
            DestroyScene();
            if (file != null && file.IsOpen())
            {
                UInt16 usNumber = 0;
                file.ReadUShort(ref usNumber);

                for (UInt16 i = 0; i < usNumber; i++)
                {
                    CSceneInfo sceneinfo = new CSceneInfo();
                    sceneinfo.Load(ref file);
                    RegistSceneInfo(sceneinfo);
                }
                s_SceneInfoMap.Sort();
                return true;
            }
            return false;
        }

        public CSceneInfo GetSceneByID(byte nID)
        {
            if (s_SceneInfoMap != null && s_SceneInfoMap.Contains(nID))
            {
                return (CSceneInfo)s_SceneInfoMap[nID];
            }
            else
            {
                return null;
            }
        }

        public CSceneInfo RandomScene()
        {
            if (s_SceneInfoMap != null)
            {
                int count = s_SceneInfoMap.Count;
                if (count > 0)
                {
                    int i = 0;
                    if (count > 1)
                    {
                        i = 1;// UnityEngine.Random.Range(0, count - 1);
                    }
                    int n = 0;
                    foreach (byte id in s_SceneInfoMap.Keys)
                    {
                        if (n == i)
                        {
                            return (CSceneInfo)s_SceneInfoMap[id];
                        }
                        n++;
                    }
                }
            }
            return null;
        }

        public XQHashtable GetAllScene()
        {
            return s_SceneInfoMap;
        }

        void RegistSceneInfo(CSceneInfo sceneinfo)
        {
            if (s_SceneInfoMap != null && sceneinfo != null)
            {
                if (s_SceneInfoMap.Contains(sceneinfo.m_nSceneID))
                {
                    //Debug.Log("RegistSceneInfo Duplicate,nType:" + sceneinfo.m_nSceneID);
                }
                else
                {
                    s_SceneInfoMap.Add(sceneinfo.m_nSceneID, sceneinfo);
                }
            }
        }

        void DestroyScene()
        {
            if (s_SceneInfoMap != null)
            {
                s_SceneInfoMap.Clear();
            }
        }
    }
}
