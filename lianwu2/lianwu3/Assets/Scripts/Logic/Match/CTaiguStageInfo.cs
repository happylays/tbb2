using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using LoveDance.Client.Common;
using LoveDance.Client.Loader;
using UnityEngine;

public class CTaiguShowTime
{
    public int BeginTime = 0;
    public int EndTime = 0;
}

public enum TaiguBallType : byte
{
    None,

    Red,
    Blue,
    RBMix,
    HoldBegin,
    Holding,
    HoldEnd,

    Max,
};

public class CTaiguStageInfo : CMatchStageInfo
{
    public List<TaiguRoundInfo> RoundInfoList = new List<TaiguRoundInfo>();
    public List<CTaiguShowTime> ShowTimeList = new List<CTaiguShowTime>();

    public override IEnumerator Load(string strFileName)
    {
        byte[] gtBytes = null;

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
        

        if (gtBytes != null)
        {
            LoadStageInfo(gtBytes);
        }
        gtBytes = null;
        //Messenger.Broadcast(MessangerEventDef.LOAD_ONEASSET_FINISH, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    public override void LoadStageInfo(byte[] stageInfo)
    {
        //SetMatchValue();

        //return;

        using (MemoryStream memStream = new MemoryStream(stageInfo))
        {
            using (StreamReader sr = new StreamReader(memStream, CommonFunc.GetCharsetEncoding()))
            {
                char[] trimStart = { ' ', '\t' };
                char[] trimEnd = { ' ', '\r', '\n', '\t' };
                List<string> roundNote = new List<string>();

                string stringLine = null;
                while ((stringLine = sr.ReadLine()) != null)
                {
                    if (stringLine != "")
                    {
                        stringLine.TrimEnd(trimEnd);
                        stringLine.TrimStart(trimStart);

                        if (BeginWithFlag(stringLine, '#'))
                        {
                            StageTag tag = AnalyseTag(ref stringLine, '#');
                            if (tag == StageTag.Round)
                            {
                                stringLine = stringLine.Replace(" ", "");
                                string strHead = "";
                                string strNote = "";
                                SeparateString(stringLine, ':', ref strHead, ref strNote);
                                roundNote.Add(strNote);
                            }
                            else if (tag == StageTag.PatEnd)
                            {
                                AnalysisRoundNote(roundNote);
                                roundNote.Clear();
                            }
                        }
                        else if (BeginWithFlag(stringLine, '@'))
                        {
                            //Show time
                            stringLine = stringLine.Replace(" ", "");
                            string strHead = "";
                            string strBegin = "";
                            string strEnd = "";
                            SeparateString(stringLine, ':', ref strHead, ref strBegin, ref strEnd);

                            int nBegin = Convert.ToInt32(strBegin);
                            int nEnd = Convert.ToInt32(strEnd);
                            if (nBegin > 0 && nBegin < nEnd)
                            {
                                CTaiguShowTime showTime = new CTaiguShowTime();
                                showTime.BeginTime = nBegin;
                                showTime.EndTime = nEnd;
                                ShowTimeList.Add(showTime);
                            }
                        }
                        else
                        {
                            string key = "";
                            string value = "";
                            SeparateString(stringLine, ':', ref key, ref value);

                            key = key.ToUpper();
                            SetMatchValue(key, value);
                        }
                    }
                }
                sr.Close();
            }
            memStream.Close();
        }
    }

    void AnalysisRoundNote(List<string> roundNote)
    {
        CTaiguBeatResult allComboResult = new CTaiguBeatResult();
        m_AllCombo = 0;

        for (int i = 0; i < roundNote.Count; ++i)
        {
            string stringNote = roundNote[i];
            TaiguRoundInfo roundInfo = new TaiguRoundInfo();

            for (int index = 0; index < stringNote.Length; ++index)
            {
                bool bValid = true;
                switch (stringNote[index])
                {
                    case '1':
                        roundInfo.BallList.Add(TaiguBallType.Red);
                        break;
                    case '2':
                        roundInfo.BallList.Add(TaiguBallType.Blue);
                        break;
                    case '3':
                        roundInfo.BallList.Add(TaiguBallType.RBMix);
                        break;
                    case '8':
                        roundInfo.BallList.Add(TaiguBallType.HoldBegin);
                        ++index;
                        bool hasOK = false;
                        for (; index < stringNote.Length; ++index)
                        {
                            switch (stringNote[index])
                            {
                                case '0':
                                    roundInfo.BallList.Add(TaiguBallType.Holding);
                                    break;
                                case '9':
                                    hasOK = true;
                                    roundInfo.BallList.Add(TaiguBallType.HoldEnd);
                                    break;
                                default:
                                    UnityEngine.Debug.Log("CTaiguStageInfo AnalysisRoundNote, Node Value is " + stringNote[index] + ", can not between 8~9");
                                    break;
                            }
                            if (hasOK)
                            {
                                break;
                            }
                        }
                        break;
                    default:
                        roundInfo.BallList.Add(TaiguBallType.None);
                        bValid = false;
                        break;
                }

                if (bValid)
                {
                    allComboResult.AddBeatCheck(0, BeatResultRank.None);
                    m_AllCombo += allComboResult.LatestBeatMark.Value;

                    roundInfo.m_bAllEmpty = false;
                }
            }

            RoundInfoList.Add(roundInfo);
        }
    }
}


public class TaiguRoundInfo
{
    public List<TaiguBallType> BallList = new List<TaiguBallType>();
    public bool m_bAllEmpty = true;
}
