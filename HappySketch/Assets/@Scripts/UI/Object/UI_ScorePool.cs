using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Define;

public class UI_ScorePool : UI_BaseObject
{
    [SerializeField, ReadOnly]
    private TextMeshProUGUI[] leftScores;

    [SerializeField, ReadOnly]
    private TextMeshProUGUI[] rightScores;

    private int leftIndex;
    private int rightIndex;

    private Camera leftCamera;
    private Camera rightCamera;

    private void Reset()
    {
        TextMeshProUGUI[] scores = GetComponentsInChildren<TextMeshProUGUI>(true);
        leftScores = new TextMeshProUGUI[scores.Length / 2];
        rightScores = new TextMeshProUGUI[scores.Length / 2];

        Array.Copy(scores, 0, leftScores, 0, scores.Length / 2);
        Array.Copy(scores, scores.Length / 2, rightScores, 0, scores.Length / 2);
    }

    public override void SetInfo(UIParam param = null)
    {
        base.SetInfo(param);

        if (param is UIScorePoolParam uIScorePoolParam)
        {
            leftCamera = uIScorePoolParam.leftCamera;
            rightCamera = uIScorePoolParam.rightCamera;
        }
    }

    // Param으로 정의?
    public void ShowScores(ETeamType teamType, Vector3[] itemPoses, int[] scores)
    {
        for (int i = 0; i < scores.Length; ++i)
        {
            ShowScore(teamType, itemPoses[i], scores[i]);
        }
    }

    public void ShowScore(ETeamType teamType, Vector3 itemPos, int score)
    {
        TextMeshProUGUI showText = null;
        Vector3 uiPos = Vector3.zero;

        switch (teamType)
        {
            case ETeamType.Left:
                showText = leftScores[leftIndex];
                uiPos = leftCamera.WorldToViewportPoint(itemPos);
                leftIndex = (leftIndex + 1) % leftScores.Length;
                break;

            case ETeamType.Right:
                showText = rightScores[rightIndex];
                uiPos = rightCamera.WorldToViewportPoint(itemPos);
                rightIndex = (rightIndex + 1) % rightScores.Length;
                break;
        }

        showText.text = score.ToString();
        showText.rectTransform.position = uiPos;
    }
}
