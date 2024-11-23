using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TitleScene : UI_BaseScene
{
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private GameObject playerPopUp;

    [SerializeField]
    private GameObject[] players;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvasNotOverlay(gameObject, false);
        Managers.UI.SetSceneUI(this);

        playButton.onClick.AddListener(OnClickPlayButton);

        return true;
    }

    private void OnClickPlayButton()
    {
        UIFadeEffectParam param = new UIFadeEffectParam(null, OpenPopup, null);
        Managers.UI.OpenPopupUI<UI_FadeEffectPopup>(param);
    }

    private void OpenPopup()
    {
        playerPopUp.SetActive(true);

        foreach (GameObject player in players)
        {
            player.SetActive(true);
        }

        ChangeSceneAfterTime(Define.EScene.GameScene, 3.0f);
    }
}
