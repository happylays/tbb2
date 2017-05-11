using System;
using System.Collections;
using System.Collections.Generic;
using LoveDance.Client.Common;

namespace LoveDance.Client.Data.Model
{
    public class CModelInfoManager
    {

        private XQHashtable s_ModelInfoMap = new XQHashtable();

        public bool Load(XQFileStream file)
        {
            DestroyModel();
            if (file != null && file.IsOpen())
            {
                UInt16 usNumber = 0;
                file.ReadUShort(ref usNumber);

                for (UInt16 i = 0; i < usNumber; i++)
                {
                    CModelInfo modelinfo = new CModelInfo();
                    modelinfo.Load(ref file);
                    RegistModelInfo(modelinfo);
                }
                s_ModelInfoMap.Sort();
                return true;
            }
            return false;
        }

        public CModelInfo GetModelByID(byte nID)
        {
            if (s_ModelInfoMap != null && s_ModelInfoMap.Contains(nID))
            {
                return (CModelInfo)s_ModelInfoMap[nID];
            }
            else
            {
                return null;
            }
        }

        public CModelInfo RandomModel()
        {
            if (s_ModelInfoMap != null)
            {
                int count = s_ModelInfoMap.Count;
                if (count > 0)
                {
                    int i = 0;
                    if (count > 1)
                    {
                        i = 1;//UnityEngine.Random.Range(0,count-1);
                    }
                    int n = 0;
                    foreach (byte id in s_ModelInfoMap.Keys)
                    {
                        if (n == i)
                        {
                            return (CModelInfo)s_ModelInfoMap[id];
                        }
                        n++;
                    }
                }
            }
            return null;
        }

        public XQHashtable GetAllModel()
        {
            return s_ModelInfoMap;
        }

        /// <summary>
        /// 获取对应的模式
        /// </summary>
        /// <param name="isSrper">是否是超级</param>
        /// <returns></returns>
        public List<CModelInfo> GetModeList(SongModeType songModeType)
        {
            List<CModelInfo> modeList = new List<CModelInfo>();

            foreach (DictionaryEntry mode in s_ModelInfoMap)
            {
                CModelInfo modeInfo = (CModelInfo)mode.Value;

                switch (songModeType)
                {
                    case SongModeType.All:
                        modeList.Add(modeInfo);
                        break;
                    case SongModeType.Base:
                        if (modeInfo.m_nModelID > (byte)SongMode.BasicMax &&
                            modeInfo.m_nModelID < (byte)SongMode.Super_Max)
                        {
                            continue;
                        }
                        else
                        {
                            modeList.Add(modeInfo);
                        }
                        break;
                    case SongModeType.Super:
                        if (modeInfo.m_nModelID > (byte)SongMode.BasicMax &&
                            modeInfo.m_nModelID < (byte)SongMode.Super_Max)
                        {
                            modeList.Add(modeInfo);
                        }
                        break;
                }
            }

            return modeList;
        }

        void RegistModelInfo(CModelInfo modelinfo)
        {
            if (s_ModelInfoMap != null && modelinfo != null)
            {
                if (s_ModelInfoMap.Contains(modelinfo.m_nModelID))
                {
                    //Debug.Log("RegistModelInfo Duplicate,nType:" + modelinfo.m_nModelID);
                }
                else
                {
                    s_ModelInfoMap.Add(modelinfo.m_nModelID, modelinfo);
                }
            }
        }

        void DestroyModel()
        {
            if (s_ModelInfoMap != null)
            {
                s_ModelInfoMap.Clear();
            }
        }
    }
}