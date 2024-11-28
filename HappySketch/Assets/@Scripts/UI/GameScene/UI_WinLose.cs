using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_WinLose : UI_BaseObject
{
    [SerializeField, ReadOnly] private Image winImage;
    [SerializeField, ReadOnly] private Image loseImage;
    [SerializeField, ReadOnly] private Image goalImage;
    [SerializeField, ReadOnly] private Image[] winCount = new Image[3];

    [SerializeField, ReadOnly] private Image backgroundImage;
    [SerializeField, ReadOnly] private UI_TextMeshProEffect goalText;

    private void Reset()
    {
        backgroundImage = GetComponentInChildren<Image>();

        //leftImage = Util.FindChild<UI_ImageEffect>(gameObject, "Img_Left");
        //rightImage = Util.FindChild<UI_ImageEffect>(gameObject, "Img_Right");

        goalText = Util.FindChild<UI_TextMeshProEffect>(gameObject, "Txt_Goal");
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //winSprite = Managers.Resource.Load<Sprite>($"{LoadPath.UI_TEXTURE_PATH}/ActionText_Victory");
        //loseSprite = Managers.Resource.Load<Sprite>($"{LoadPath.UI_TEXTURE_PATH}/ActionText_Defeat");

        return true;
    }

    public override void SetInfo(UIParam param = null)
    {
        base.SetInfo(param);

        if(param is UIWinLoseParam winLoseParam)
        {
            bool isLeftWin = winLoseParam.WinTeam == ETeamType.Left;

            //leftImage.SetSprite(isLeftWin ? winSprite : loseSprite);
            //rightImage.SetSprite(!isLeftWin ? winSprite : loseSprite);

            goalText.SetPosition(isLeftWin ? new Vector3(-480f, 0f) : new Vector3(480f, 0f));
            goalText.gameObject.SetActive(true);
        }
    }

    private IEnumerator FadeOutCoroutine(Image targetImage, float fadeTime)
    {
        targetImage.color = Color.clear;

        float time = 0f;

        while(time < fadeTime)
        {
            time += Time.deltaTime;

            targetImage.color = Color.Lerp(targetImage.color, Color.black / 2f, time / fadeTime);

            yield return null;
        }

        //leftImage.OpenImageEffectUI();
        //rightImage.OpenImageEffectUI();
    }

    public void Win()
    {

    }

    public void Lose()
    {

    }
}
