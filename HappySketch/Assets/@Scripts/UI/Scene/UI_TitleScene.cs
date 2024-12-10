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

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvasNotOverlay(gameObject, false);
        Managers.UI.SetSceneUI(this);

        playButton.onClick.AddListener(OnClickPlayButton);

        return true;
    }

    private void Reset()
    {
        playButton = Util.FindChild<Button>(gameObject, "Btn_Play", true);
        playerPopUp = Util.FindChild<GameObject>(gameObject, "Img_BackGround", true);
    }

    private void OnClickPlayButton()
    {
        Managers.Sound.PlaySfx(Define.ESfxSoundType.UI_StartGame);
        UIFadeEffectParam param = new UIFadeEffectParam(null, OpenPopup, null);
        Managers.UI.OpenPopupUI<UI_FadeEffectPopup>(param);
    }

    private void OpenPopup()
    {
        playerPopUp.SetActive(true);

        // 플레이어 동적 생성
        GameObject leftPlayer = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_PLAYER_PATH}/UILeftPlayer");
        GameObject rightPlayer = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_PLAYER_PATH}/UIRightPlayer");

        leftPlayer.transform.position = new Vector3(-4.2f, -1.3f, 0f);
        rightPlayer.transform.position = new Vector3(4.2f, -1.3f, 0f);

        leftPlayer.GetComponent<UI_Player>().SetInfo(true);
        rightPlayer.GetComponent<UI_Player>().SetInfo(true);

        ChangeSceneAfterTime(Define.EScene.GameScene, 3.0f);
    }
}
