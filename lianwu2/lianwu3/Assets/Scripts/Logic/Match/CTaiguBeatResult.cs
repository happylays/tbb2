using System.Collections.Generic;
using LoveDance.Client.Common;
using LoveDance.Client.Logic.Room;
///using LoveDance.Client.Logic.FashionValue;

public class CTaiguBeatResult
{
    public WrappedInt ComnoCount
    {
        get
        {
            return mComboCount;
        }
    }

    public WrappedInt LatestBeatMark
    {
        get
        {
            return mLatestBeatMark;
        }
    }

    public WrappedInt LatestBeatRank
    {
        get
        {
            return mLatestBeatRank;
        }
    }

    public WrappedInt LatestRoundMark
    {
        get
        {
            return mLatestRoundMark;
        }
    }

    public List<WrappedInt> LatestRoundRank
    {
        get
        {
            return mLatestRoundRank;
        }
    }

    const int mComboBonus = 10;		// percent

    float mTotalRange = 0f;
    float mPerfectRange = 0f;
    float mCoolRange = 0f;
    float mGoodRange = 0f;

    int[] mBaseMark = 
	{
		0,			// miss
		250,		// bad
		300,		// good
		400,		// cool
		500			// perfect
	};

    WrappedInt mComboCount = new WrappedInt();

    WrappedInt mLatestBeatMark = new WrappedInt();
    WrappedInt mLatestBeatRank = new WrappedInt();

    WrappedInt mLatestRoundMark = new WrappedInt();
    List<WrappedInt> mLatestRoundRank = new List<WrappedInt>();

    private List<uint> mAdditionList = null;

    public void InitResult(float range)
    {
        mTotalRange = range;
        mPerfectRange = CommonDef.BEAT_SCOPE_PERFECT * mTotalRange / CommonDef.BEAT_SCOPE_TOTAL;
        mCoolRange = CommonDef.BEAT_SCOPE_COOL * mTotalRange / CommonDef.BEAT_SCOPE_TOTAL;
        mGoodRange = CommonDef.BEAT_SCOPE_GOOD * mTotalRange / CommonDef.BEAT_SCOPE_TOTAL;

        mComboCount.Value = 0;

        mLatestBeatMark.Value = 0;
        mLatestBeatRank.Value = (int)BeatResultRank.None;

        mLatestRoundMark.Value = 0;
        mLatestRoundRank.Clear();

        ///mAdditionList = FashionValueData.GetAdditionData();
    }

    public bool InCheckRange(float range)
    {
        if (range >= -mTotalRange && range <= mTotalRange)
        {
            return true;
        }

        return false;
    }

    public BeatResultRank AddBeatCheck(float offset, BeatResultRank headRank)
    {
        BeatResultRank rank = BeatResultRank.None;
        if (headRank != BeatResultRank.None)
        {
            rank = headRank;
            switch (rank)
            {
                case BeatResultRank.Perfect:
                case BeatResultRank.Cool:
                case BeatResultRank.Good:
                    ++mComboCount.Value;
                    break;
                case BeatResultRank.Bad:
                case BeatResultRank.Miss:
                    mComboCount.Value = 0;
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (offset >= -mPerfectRange && offset <= mPerfectRange)
            {
                rank = BeatResultRank.Perfect;
                ++mComboCount.Value;
            }
            else if (offset >= -mCoolRange && offset <= mCoolRange)
            {
                rank = BeatResultRank.Cool;
                ++mComboCount.Value;
            }
            else if (offset >= -mGoodRange && offset <= mGoodRange)
            {
                rank = BeatResultRank.Good;
                ++mComboCount.Value;
            }
            else if (offset >= -mTotalRange && offset <= mTotalRange)
            {
                rank = BeatResultRank.Bad;
                mComboCount.Value = 0;
            }
            else
            {
                rank = BeatResultRank.Miss;
                mComboCount.Value = 0;
            }
        }

        int comboLevel = (mComboCount.Value > 100 ? 10 : mComboCount.Value / 10);
        int markIndex = (int)rank - 1;
        int rankMark = mBaseMark[markIndex];
        ///rankMark += FashionValueData.GetAdditionFashionValue(rank, mAdditionList);
        mLatestBeatMark.Value = rankMark + rankMark * comboLevel * mComboBonus / 100;
        mLatestBeatRank.Value = (int)rank;

        mLatestRoundMark.Value += mLatestBeatMark.Value;
        mLatestRoundRank.Add(mLatestBeatRank.Clone());

        return rank;
    }

    public void AddBeatMiss()
    {
        mComboCount.Value = 0;

        int markIndex = (int)BeatResultRank.Miss - 1;
        int rankMark = mBaseMark[markIndex];
        mLatestBeatMark.Value = rankMark;
        mLatestBeatRank.Value = (int)BeatResultRank.Miss;

        mLatestRoundMark.Value += mLatestBeatMark.Value;
        mLatestRoundRank.Add(mLatestBeatRank.Clone());
    }

    public void StartNewRound()
    {
        mLatestRoundMark.Value = 0;
        mLatestRoundRank.Clear();
    }
}
