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

    private void StartStage()
    {
        Managers.Game.StartGame();
        // OnStageInstruction?.Invoke();
    }
    #region OnClick Event
    public void OnClickExit()
    {
        StartStage();

        gameObject.SetActive(false);
    }
    #endregion
}
