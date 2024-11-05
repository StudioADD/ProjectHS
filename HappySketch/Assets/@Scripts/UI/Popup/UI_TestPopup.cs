using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TestPopup : UI_BasePopup
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void OpenPopupUI()
    {
        base.OpenPopupUI();

        Debug.Log("Open!!");
    }


    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }

    public override void DeActivePopup()
    {
        base.DeActivePopup();
    }

    public override void SetInfo(UIParam param)
    {
        Debug.Log("SetInfo!");
    }
}