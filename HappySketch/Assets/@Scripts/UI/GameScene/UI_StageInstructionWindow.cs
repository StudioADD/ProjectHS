using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI_StageInstructionWindow : InitBase
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Resource.Instantiate($"{PrefabPath.UI_STAGE_PATH}/UI_Rule_{Managers.Game.GetCurrStageType()}", transform);

        return true;
    }

    private void OnEnable()
    {
        Managers.Input.OnAnyKeyEntered += OnStartStage;
    }

    public void SetInfo(EStageType stageType)
    {

    }

    private void StartStage()
    {
        Managers.Game.SetStageInfo();
        gameObject.SetActive(false);
    }

    public void OnStartStage()
    {
        UIFadeEffectParam param = new UIFadeEffectParam(() => { return !gameObject.activeSelf; }, StartStage, null);
        Managers.UI.OpenPopupUI<UI_FadeEffectPopup>(param);
    }

    private void OnDisable()
    {
        Managers.Input.OnAnyKeyEntered -= OnStartStage;
    }
}
