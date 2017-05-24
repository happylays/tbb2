using UnityEngine;

/// <summary>
/// Trivial script that fills the label's contents gradually, as if someone was typing.
/// </summary>

[RequireComponent(typeof(UILabel))]
[AddComponentMenu("NGUI/Examples/Typewriter Effect")]
public class TypewriterEffect : MonoBehaviour
{
	public enum WriteType
	{
		PerCharacter,
		Line,
	}

	public WriteType m_WriteType = WriteType.PerCharacter;
	public int charsPerSecond = 40;

	UILabel mLabel;
	string mText;
	int mOffset = 0;
	float mNextChar = 0f;
	int mLine = 0;
	int m_LineNum = 0;
	string[] mSplitLine = null;

	void Update ()
	{
		if (mLabel == null)
		{
			mLabel = GetComponent<UILabel>();
			mLabel.supportEncoding = false;
			mLabel.symbolStyle = UIFont.SymbolStyle.None;
			mText = mLabel.font.WrapText(mLabel.text, mLabel.lineWidth / mLabel.cachedTransform.localScale.x, mLabel.maxLineCount, false, UIFont.SymbolStyle.None);

			mSplitLine = mText.Split('\n');
			m_LineNum = mSplitLine.Length;
		}

		if (mOffset < mText.Length)
		{
			if (mNextChar <= Time.time)
			{
				if (m_WriteType == WriteType.PerCharacter)
				{
					charsPerSecond = Mathf.Max(1, charsPerSecond);

					// Periods and end-of-line characters should pause for a longer time.
					float delay = 1f / charsPerSecond;

					char c = mText[mOffset];
					if (c == '.' || c == '\n' || c == '!' || c == '?') delay *= 4f;

					mNextChar = Time.time + delay;
					mLabel.text = mText.Substring(0, ++mOffset);
				}
				else
				{
					float delay = 1f / charsPerSecond;

					if (mLine  == m_LineNum)       //当显示的最后一行时
					{
						mLabel.text = mText;
						mOffset = mText.Length;  //显示的最后一行时，offset延迟赋值 
					}
					else
					{
						mOffset += mSplitLine[mLine].Length;
						if (mLine + 1 != m_LineNum)
						{
							mOffset += 1;
						}
						mLabel.text = mText.Substring(0, mOffset);

						if (mLine + 1 == m_LineNum)
						{
							mOffset -= mSplitLine[mLine].Length;
						}
					}

					if (mLine < m_LineNum)
					{
						delay *= (mSplitLine[mLine].Length + 4);
						mNextChar = Time.time + delay;
						mLine++;
					}
				}
			}
		}
		else Destroy(this);
	}
}