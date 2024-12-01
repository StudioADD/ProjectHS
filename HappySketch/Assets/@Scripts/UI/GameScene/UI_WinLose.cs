using JetBrains.Annotations;
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
    [SerializeField] private Image goalImage;

    private const int COUNT = 3;
    [SerializeField, ReadOnly] private Image[] winCount = new Image[COUNT];

    private const float FADE_TIME = 3f;

    private void Reset()
    {
        winImage = Util.FindChild<Image>(gameObject, "win");
        loseImage = Util.FindChild<Image>(gameObject, "lose");
        //goalImage = Util.FindChild<Image>(transform.parent.gameObject, "Img_Goal_Left");

        for(int i = 0; i < COUNT; ++i)
        {
            winCount[i] = Util.FindChild<Image>(gameObject, i.ToString());
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void SetInfo(UIParam param = null)
    {
        base.SetInfo(param);

        if(param is UIWinLoseParam winLoseParam)
        {
            bool isLeftWin = winLoseParam.WinTeam == ETeamType.Left;
        }
    }

    private IEnumerator FadeOutCoroutine(Image targetImage, float fadeTime)
    {
        targetImage.gameObject.SetActive(true);

        targetImage.color = Color.clear;

        float elapsed = 0f;

        while(elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            targetImage.color = Color.Lerp(targetImage.color, Color.white, elapsed / fadeTime);

            yield return null;
        }

        targetImage.color = Color.white;
    }

    public void Win(int winCount)
    {
        StartCoroutine(FadeOutCoroutine(winImage, FADE_TIME));
        StartCoroutine(FadeOutCoroutine(this.winCount[winCount], FADE_TIME));
    }

    public void Lose(int winCount)
    {
        StartCoroutine(FadeOutCoroutine(loseImage, FADE_TIME));
        StartCoroutine(FadeOutCoroutine(this.winCount[winCount], FADE_TIME));
    }
    
    public void ShowGoalImage()
    {
        StartCoroutine(FadeOutCoroutine(goalImage, FADE_TIME));
    }
}
