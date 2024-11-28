using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Define;

public class SharkAvoidanceModel : ModelBase
{
    private enum CoroutineType
    {
        Left,
        Right,
        Bar,
        Last
    }

    private class Ratio
    {
        public float ratio;
        public float currRatio;

        public Ratio(float ratio = 0f, float currRatio = 0f)
        {
            this.ratio = ratio;
            this.currRatio = currRatio;
        }
    }

    private int leftItemCount;
    private int rightItemCount;

    private Ratio leftRatio = new Ratio();
    private Ratio rightRatio = new Ratio();
    private Ratio progressRatio = new Ratio();

    private Coroutine[] coroutines = new Coroutine[(int)CoroutineType.Last];

    public event UnityAction<float> onLeftRatioChanged;
    public event UnityAction<float> onRightRatioChanged;

    private const float PROGRESS_TIME = 3f;

    public SharkAvoidanceModel() : base()
    {
        
    }

    public void SetLeftItemCount(int itemCount)
    {
        leftItemCount = itemCount;
    }

    public void SetRightItemCount(int itemCount)
    {
        rightItemCount = itemCount;
    }

    public void SetLeftRatio(float ratio)
    {
        leftRatio.ratio = ratio;

        if (coroutines[(int)CoroutineType.Left] != null)
            CoroutineHelper.StopCoroutine(coroutines[(int)CoroutineType.Left]);

        coroutines[(int)CoroutineType.Left] = CoroutineHelper.StartCoroutine(SetProgressCoroutine(leftRatio, onLeftRatioChanged, CoroutineType.Left));
    }

    public void SetRightRatio(float ratio)
    {
        rightRatio.ratio = ratio;

        if (coroutines[(int)CoroutineType.Right] != null)
            CoroutineHelper.StopCoroutine(coroutines[(int)CoroutineType.Right]);

        coroutines[(int)CoroutineType.Right] = CoroutineHelper.StartCoroutine(SetProgressCoroutine(rightRatio, onRightRatioChanged, CoroutineType.Right));
    }

    private IEnumerator SetProgressCoroutine(Ratio ratio, UnityAction<float> action, CoroutineType coroutineType)
    {
        float elapsed = 0f;

        while(elapsed < PROGRESS_TIME)
        {
            elapsed += Time.deltaTime;

            ratio.currRatio = Mathf.Lerp(ratio.currRatio, ratio.ratio, elapsed / PROGRESS_TIME);

            action?.Invoke(ratio.currRatio);

            yield return null;
        }

        action?.Invoke(ratio.ratio);

        coroutines[(int)coroutineType] = null;
    }

    public void Clear()
    {
        foreach(Coroutine coroutine in coroutines)
        {
            if (coroutine != null)
                CoroutineHelper.StopCoroutine(coroutine);
        }
    }
}