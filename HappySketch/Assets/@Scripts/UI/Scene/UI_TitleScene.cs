using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : UI_BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    // Test
    public void OnClickGoToGameScene()
    {
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }

    public void OnClickGoToRuleScene()
    {
        Managers.Scene.LoadScene(Define.EScene.RuleScene);
    }
    public void OnClickGoToIntroPlayerScene()
    {
        Managers.Scene.LoadScene(Define.EScene.IntroPlayerScene);
    }

    public void OnClickedRedButton()
    {
        //UIFadeEffectParam param = new UIFadeEffectParam(null, Foo1, Foo2);
        Managers.UI.OpenPopupUI<UI_TestPopup>();
    }

    public void OnClickedArrowButton()
    {
        Debug.Log("OnClickedArrowButton");
    }

    private void Foo1()
    {
        Debug.Log("Foo1");
    }

    private void Foo2()
    {
        Debug.Log("Foo2");
    }
}
