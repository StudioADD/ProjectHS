using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageInstructionWindow : InitBase
{
    public event Action OnStageInstruction = null;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    #region OnClick Event
    public void OnClickExit()
    {
        OnStageInstruction?.Invoke();

        gameObject.SetActive(false);
    }
    #endregion
}
