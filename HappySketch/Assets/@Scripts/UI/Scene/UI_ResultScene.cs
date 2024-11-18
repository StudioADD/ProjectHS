using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ResultScene : UI_BaseScene
{
    [SerializeField]
    private GameObject leftPlayer;

    [SerializeField]
    private GameObject rightPlayer;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvasNotOverlay(gameObject, false);
        Managers.UI.SetSceneUI(this);

        return true;
    }

    public void SetActivePlayer(bool isLeft)
    {
        leftPlayer.SetActive(isLeft);
        rightPlayer.SetActive(!isLeft);
    }
}

