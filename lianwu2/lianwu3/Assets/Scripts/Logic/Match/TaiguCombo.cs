using UnityEngine;
using LoveDance.Client.Common;


public class TaiguCombo : MonoBehaviour
{
    [SerializeField]
    float m_ComboSize = 0;
    [SerializeField]
    Vector2 m_NumberSize = Vector2.zero;
    [SerializeField]
    Vector2 m_TextureSize = Vector2.zero;

    [SerializeField]
    Renderer m_ComboRenderer = null;
    [SerializeField]
    Renderer[] m_NumberRenderer = null;

    [SerializeField]
    Animation m_EffectAni = null;

    int mDivisor = 1;

    float mComboOffset = 0f;
    Vector2 mNumOffset = Vector2.zero;

    void Awake()
    {
        for (int i = 1; i < m_NumberRenderer.Length; ++i)
        {
            mDivisor *= 10;
        }

        mComboOffset = m_ComboSize / m_TextureSize.y;
        mNumOffset.x = m_NumberSize.x / m_TextureSize.x;
        mNumOffset.y = m_NumberSize.y / m_TextureSize.y;
    }

    void SetNumRenderer(int index, int numValue, int numLevel, int comboLevel)
    {
        if (index < m_NumberRenderer.Length && numValue >= 0 && numValue < 10)
        {
            Renderer r = m_NumberRenderer[index];
            r.gameObject.SetActive(true);
            r.material.SetFloat("_UVOffsetx", numLevel * mNumOffset.x);
            r.material.SetFloat("_UVOffsety", (-1) * numValue * mNumOffset.y);
        }

        m_ComboRenderer.material.SetFloat("_UVOffsety", (-1) * comboLevel * mComboOffset);
    }

    public void ChangeCount(int comboCount)
    {
        ContinuousBeatBonus beatBonus = CTaiguMatch.CalcuTaiguComboBonus(comboCount);
        if (beatBonus >= ContinuousBeatBonus.Lv1)
        {
            int numLevel = 0;
            int comboLevel = 0;

            if (beatBonus == ContinuousBeatBonus.Lv5)
            {
                numLevel = 2;
                comboLevel = 4;
            }
            else if (beatBonus == ContinuousBeatBonus.Lv4)
            {
                numLevel = 1;
                comboLevel = 3;
            }
            else if (beatBonus == ContinuousBeatBonus.Lv3)
            {
                comboLevel = 2;
            }
            else if (beatBonus == ContinuousBeatBonus.Lv2)
            {
                comboLevel = 1;
            }

            int occupyIndex = 0;
            int divisor = mDivisor;
            for (int index = 0; index < m_NumberRenderer.Length; ++index)
            {
                int numValue = comboCount / divisor;
                if (numValue > 0)
                {
                    SetNumRenderer(occupyIndex, numValue, numLevel, comboLevel);
                    ++occupyIndex;
                }
                else
                {
                    if (occupyIndex != 0)
                    {
                        SetNumRenderer(occupyIndex, numValue, numLevel, comboLevel);
                        ++occupyIndex;
                    }
                }

                comboCount %= divisor;
                divisor /= 10;
            }

            for (; occupyIndex < m_NumberRenderer.Length; ++occupyIndex)
            {
                m_NumberRenderer[occupyIndex].gameObject.SetActive(false);
            }
        }
    }

    public void PlayEffect()
    {
        m_EffectAni.Stop();
        m_EffectAni.Play();
    }
}
