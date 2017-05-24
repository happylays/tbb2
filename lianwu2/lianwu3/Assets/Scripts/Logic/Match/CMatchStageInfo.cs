using System;
using System.Collections;
using System.Collections.Generic;


public abstract class CMatchStageInfo
{
    public abstract IEnumerator Load(string strFileName);
    public abstract void LoadStageInfo(byte[] stageInfo);

    public virtual void ResetStageInfoData()
    {
    }

    public enum StageTag : byte
    {
        None = 0x0,
        Round,
        PatStart,
        PatEnd,
    };

    public float RoundTime
    {
        get
        {
            return ((60 * 4 * mBeatN) / (mBeatD * mBPM));
        }
    }

    public float BeatTime
    {
        get
        {
            return (60 / mBPM);
        }
    }

    public string mMusicFile = "";

    public int mBeatN = 4;		//分子
    public int mBeatD = 4;		//分母
    public int m_AllCombo = 0;	// 全P分数

    public float mBPM = 0f;
    public float mOffset = 0f;
    public float mOffset_2 = 0f;//for HeartBeat Mode Only
    public float mKSpeed = 1f;

    public float mMatchTime = 0f;
    public float mDanceTime = 0f;
    public List<int[]> mShowRounds = new List<int[]>();

    public void SetMatchValue()
    {
        mMusicFile = "song2246";

        mBPM = 128;

        mOffset = 9.615f;

        mKSpeed = 0.9f;

        mMatchTime = 102;

        mDanceTime = 9.625f;
       
    }

    public void SetMatchValue(string key, string value)
    {
        if (string.Compare(key, "TITLE") == 0)
        {
            mMusicFile = value;
        }
        else if (string.Compare(key, "BPM") == 0)
        {
            mBPM = Convert.ToSingle(value);
        }
        else if (string.Compare(key, "MEASURE") == 0)
        {
            string beatN = "";
            string beatD = "";
            SeparateString(value, '/', ref beatN, ref beatD);

            Int32.TryParse(beatN, out mBeatN);
            Int32.TryParse(beatD, out mBeatD);
        }
        else if (string.Compare(key, "OFFSET") == 0)
        {
            mOffset = Convert.ToSingle(value);
        }
        else if (string.Compare(key, "OFFSET2") == 0)
        {
            mOffset_2 = Convert.ToSingle(value);
        }
        else if (string.Compare(key, "KSPEED") == 0)
        {
            mKSpeed = Convert.ToSingle(value);
        }
        else if (string.Compare(key, "MATCHTIME") == 0)
        {
            mMatchTime = Convert.ToSingle(value);
        }
        else if (string.Compare(key, "DANGCE") == 0)
        {
            mDanceTime = Convert.ToSingle(value);
        }
        else if (key.StartsWith("SHOWTIME"))
        {
            string beginRound = "";
            string endRound = "";
            SeparateString(value, '/', ref beginRound, ref endRound);

            int[] showRound = new int[2];
            if (Int32.TryParse(beginRound, out showRound[0]) && Int32.TryParse(endRound, out showRound[1]))
            {
                mShowRounds.Add(showRound);
            }
        }
    }

    public void SeparateString(string srcString, char flag, ref string firstPart, ref string secondPart)
    {
        if (srcString != null && srcString.Length > 0)
        {
            string[] splitResult = srcString.Split(flag);
            if (splitResult.Length >= 2)
            {
                firstPart = splitResult[0];
                secondPart = splitResult[1];
            }
        }
    }

    public void SeparateString(string srcString, char flag, ref string firstPart, ref string secondPart, ref string thirdPart)
    {
        if (srcString != null && srcString.Length > 0)
        {
            string[] splitResult = srcString.Split(flag);
            if (splitResult.Length >= 3)
            {
                firstPart = splitResult[0];
                secondPart = splitResult[1];
                thirdPart = splitResult[2];
            }
        }
    }

    public bool BeginWithNum(string srcString)
    {
        if (srcString != null && srcString.Length > 0)
        {
            char ch = srcString[0];
            if (ch >= '0' && ch <= '9')
            {
                return true;
            }
        }

        return false;
    }

    public bool BeginWithFlag(string srcString, char flag)
    {
        if (srcString != null && srcString.Length > 0 && srcString[0] == flag)
        {
            return true;
        }

        return false;
    }

    public StageTag AnalyseTag(ref string srcString, char flag)
    {
        if (BeginWithFlag(srcString, flag))
        {
            srcString = srcString.Substring(1, srcString.Length - 1);
            if (BeginWithNum(srcString))
            {
                return StageTag.Round;
            }
            else
            {
                srcString.ToUpper();
                if (srcString.CompareTo("PATSTART") == 0)
                {
                    return StageTag.PatStart;
                }
                else if (srcString.CompareTo("PATEND") == 0)
                {
                    return StageTag.PatEnd;
                }
            }
        }

        return StageTag.None;
    }

    public bool IsComment(string srcString)
    {
        if (srcString.Length >= 2 && srcString[0] == '/' && srcString[1] == '/')
        {
            return true;
        }
        return false;
    }
}
