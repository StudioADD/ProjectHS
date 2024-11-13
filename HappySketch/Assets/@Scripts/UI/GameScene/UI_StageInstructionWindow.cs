using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageInstructionWindow : InitBase
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    private void StartStage()
    {
        Managers.Game.StartStage();
        gameObject.SetActive(false);
    }

    #region OnClick Event
    public void OnClickExit()
    {
        UIFadeEffectParam param = new UIFadeEffectParam(() => { return !gameObject.activeSelf; }, StartStage, null);
        Managers.UI.OpenPopupUI<UI_FadeEffectPopup>(param);
    }
    #endregion
}
