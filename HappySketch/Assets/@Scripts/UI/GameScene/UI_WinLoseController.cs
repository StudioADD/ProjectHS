using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_WinLoseController : UI_BaseObject
{
    [SerializeField, ReadOnly]
    private UI_WinLose leftWinLose;

    [SerializeField, ReadOnly]
    private UI_WinLose rightWinLose;

    [SerializeField, ReadOnly]
    private Image backGround;

    private const float FADE_TIME = 3f;

    private void Reset()
    {
        leftWinLose = Util.FindChild<UI_WinLose>(gameObject, "WinLose_Left");
        rightWinLose = Util.FindChild<UI_WinLose>(gameObject, "WinLose_Right");
        backGround = Util.FindChild<Image>(gameObject, "Img_Background");
    }

    public void EndStage(ETeamType teamType, int leftWinCount, int rightWinCount, Action onEnd)
    {
        switch(teamType)
        {
            case ETeamType.Left:
                leftWinLose.ShowGoalImage();
                break;

            case ETeamType.Right:
                rightWinLose.ShowGoalImage();
                break;
        }

        StartCoroutine(FadeOutCoroutine(FADE_TIME, teamType, leftWinCount, rightWinCount, onEnd));
    }

    private void Handle(ETeamType teamType, int leftWinCount, int rightWinCount, Action onEnd)
    {
        switch (teamType)
        {
            case ETeamType.Left:
                leftWinLose.Win(leftWinCount);
                rightWinLose.Lose(rightWinCount);
                break;

            case ETeamType.Right:
                leftWinLose.Lose(leftWinCount);
                rightWinLose.Win(rightWinCount);
                break;
        }
    }

    private IEnumerator FadeOutCoroutine(float fadeTime, ETeamType teamType, int leftWinCount, int rightWinCount, Action onEnd)
    {
        float elapsed = 0f;

        Color startColor = Color.clear;
        Color targetColor = Color.black / 2f;

        backGround.color = startColor;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            backGround.color = Color.Lerp(startColor, targetColor, elapsed / fadeTime);

            yield return null;
        }

        backGround.color = targetColor;

        Handle(teamType, leftWinCount, rightWinCount, onEnd);

        yield return new WaitForSeconds(fadeTime * 2f);

        onEnd?.Invoke();

        Managers.Resource.Destroy(gameObject);
    }
}  
