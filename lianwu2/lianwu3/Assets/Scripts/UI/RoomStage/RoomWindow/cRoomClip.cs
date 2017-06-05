using UnityEngine;
public class cRoomClip : UIClip
{
    public GameObject m_btnOk;

    public cRoomClip(GameObject mainObject)
        : base(mainObject)
    {
        //btnOk = getGameObject("medal.btnOk");

        m_btnOk = getGameObject("MainUI.0_MainUI.Anchor_br.NormalRoomContent.NormalStart.StartArea.BtnGroup.BtnStart");
    }

    public void SetMethod() 
    {
        //btnOk.SetActive(true);    
    }

    public UILabel getNameLabel(GameObject go)
    {
        return null;
    }
}
