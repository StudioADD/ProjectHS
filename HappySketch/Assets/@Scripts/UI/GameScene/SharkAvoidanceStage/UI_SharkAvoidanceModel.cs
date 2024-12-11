using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UI_SharkAvoidanceModel : UI_ModelBase
{
    private enum CoroutineType
    {
        Left,
        Right,
        Bar,
        LeftItem,
        RightItem,
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

    private Ratio leftItemRatio = new Ratio();
    private Ratio rightItemRatio = new Ratio();

    private Coroutine[] coroutines = new Coroutine[(int)CoroutineType.Last];

    public event Action<float> OnLeftRatioChanged;
    public event Action<float> OnRightRatioChanged;
    public event Action<float> OnLeftItemRatioChanged;
    public event Action<float> OnRightItemRatioChanged;

    private const float PROGRESS_TIME = 3f;

    public UI_SharkAvoidanceModel() : base()
    {
        
    }

    public void SetLeftItemCount(int itemCount)
    {
        leftItemCount = itemCount;

        float ratio = itemCount / 3f;
        leftItemRatio.ratio = ratio;

        if (coroutines[(int)CoroutineType.LeftItem] != null)
            CoroutineHelper.StopCoroutine(coroutines[(int)CoroutineType.LeftItem]);

        coroutines[(int)CoroutineType.LeftItem] = CoroutineHelper.StartCoroutine(SetProgressCoroutine(leftItemRatio, OnLeftItemRatioChanged, CoroutineType.LeftItem));
    }

    public void SetRightItemCount(int itemCount)
    {
        rightItemCount = itemCount;

        float ratio = itemCount / 3f;
        rightItemRatio.ratio = ratio;

        if (coroutines[(int)CoroutineType.RightItem] != null)
            CoroutineHelper.StopCoroutine(coroutines[(int)CoroutineType.RightItem]);

        coroutines[(int)CoroutineType.RightItem] = CoroutineHelper.StartCoroutine(SetProgressCoroutine(rightItemRatio, OnRightItemRatioChanged, CoroutineType.RightItem));
    }

    public void SetLeftRatio(float ratio)
    {
        leftRatio.ratio = ratio;

        if (coroutines[(int)CoroutineType.Left] != null)
            CoroutineHelper.StopCoroutine(coroutines[(int)CoroutineType.Left]);

        coroutines[(int)CoroutineType.Left] = CoroutineHelper.StartCoroutine(SetProgressCoroutine(leftRatio, OnLeftRatioChanged, CoroutineType.Left));
    }

    public void SetRightRatio(float ratio)
    {
        rightRatio.ratio = ratio;

        if (coroutines[(int)CoroutineType.Right] != null)
            CoroutineHelper.StopCoroutine(coroutines[(int)CoroutineType.Right]);

        coroutines[(int)CoroutineType.Right] = CoroutineHelper.StartCoroutine(SetProgressCoroutine(rightRatio, OnRightRatioChanged, CoroutineType.Right));
    }

    private IEnumerator SetProgressCoroutine(Ratio ratio, Action<float> onRatioChanged, CoroutineType coroutineType)
    {
        float elapsed = 0f;

        while(elapsed < PROGRESS_TIME)
        {
            elapsed += Time.deltaTime;

            ratio.currRatio = Mathf.Lerp(ratio.currRatio, ratio.ratio, elapsed / PROGRESS_TIME);

            onRatioChanged?.Invoke(ratio.currRatio);

            yield return null;
        }

        ratio.currRatio = ratio.ratio;
        onRatioChanged?.Invoke(ratio.ratio);

        coroutines[(int)coroutineType] = null;
    }

    public override void Clear()
    {
        foreach(Coroutine coroutine in coroutines)
        {
            if (coroutine != null)
                CoroutineHelper.StopCoroutine(coroutine);
        }
    }
}