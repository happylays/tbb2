using UnityEngine;


public enum TaiguBeatType : byte
{
    Nothing,
    Ignore,

    Left,
    Right,
    LRMix,
};


public class TaiguDrumBeat : MonoBehaviour
{
    [SerializeField]
    GameObject m_Target = null;
    [SerializeField]
    TaiguBeatType m_BeatType = TaiguBeatType.Nothing;

    void OnPress(bool isPressed)
    {
        if (isPressed)
        {
            m_Target.SendMessage("OnDrumDown", m_BeatType, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            m_Target.SendMessage("OnDrumUp", m_BeatType, SendMessageOptions.DontRequireReceiver);
        }
    }
}
