﻿using UnityEngine;
public class cCreateRoleClip : UIClip
{
    public GameObject m_btnOk;

    public cCreateRoleClip(GameObject mainObject)
        : base(mainObject)
    {
        //btnOk = getGameObject("medal.btnOk");

        //m_btnOk = getGameObject("Camera.UserAgreementUI.Acher_center.BtnConfirm");
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