using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Define;

public class SharkAvoidanceModel : ModelBase
{
    private int itemCount;
    private float leftProgressRatio;
    private float rightProgressRatio;
    private float targetRatio;

    private Coroutine setProgressCoroutine;

    public event UnityAction<float> progressEvent;

    private const float PROGRESS_TIME = 3f;

    public SharkAvoidanceModel(ETeamType teamType) : base(teamType)
    {

    }

    public void SetItemCount(int itemCount)
    {
        this.itemCount = itemCount;
    }

    public void SetLeftProgressRatio(float currRatio, float ratio)
    {
        leftProgressRatio = ratio;

        if (TeamType == ETeamType.Left)
            SetTargetRatio(currRatio, ratio);
    }

    public void SetRightProgressRatio(float currRatio, float ratio)
    {
        rightProgressRatio = ratio;

        if (TeamType == ETeamType.Right)
            SetTargetRatio(currRatio, ratio);
    }

    private void SetTargetRatio(float currRatio, float ratio)
    {
        targetRatio = ratio;

        if (setProgressCoroutine == null)
            CoroutineHelper.StartCoroutine(SetProgressCoroutine(currRatio));
    }

    private IEnumerator SetProgressCoroutine(float currRatio)
    {
        float elapsed = 0f;

        while(elapsed < PROGRESS_TIME)
        {
            elapsed += Time.deltaTime;

            currRatio = Mathf.Lerp(currRatio, targetRatio, elapsed / PROGRESS_TIME);

            progressEvent?.Invoke(currRatio);

            yield return null;
        }

        progressEvent?.Invoke(targetRatio);

        setProgressCoroutine = null;
    }
}
