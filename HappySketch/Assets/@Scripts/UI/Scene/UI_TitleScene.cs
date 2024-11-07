using MomDra;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : UI_BaseScene
{
    private PresenterBase presenter;

    [SerializeField] GameObject go;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void OnClickTest()
    {
        UIFadeEffectParam param = new UIFadeEffectParam(null, OpenPopup, null);
        Managers.UI.OpenPopupUI<UI_FadeEffectPopup>(param);
    }

    public void OpenPopup()
    {
        go.SetActive(!go.activeSelf);
    }
}
