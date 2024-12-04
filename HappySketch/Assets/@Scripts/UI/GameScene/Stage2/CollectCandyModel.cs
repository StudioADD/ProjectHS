using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Define; 

public class CollectCandyModel : ModelBase
{
    private const float SCORE_COROUTINE_TIME = 1;

    private int second = 0;
    private int minute = 0;

    public event UnityAction OnTimeChangedEvent;
    private Coroutine timeCoroutine;

    private int score;
    private int currScore;

    private int leftScore;
    private int rightScore;

    public event UnityAction<int> OnScoreChangedEvent;
    private Coroutine scoreCoroutine;

    public CollectCandyModel() : base()
    {
        
    }

    public string GetFormattedTime()
    {
        return string.Format("{0:D2}:{1:D2}", minute, second);
    }

    public void StartTimer(int minute, int second, Action onEndTimer = null)
    {
        this.minute = minute;
        this.second = second;

        timeCoroutine = CoroutineHelper.StartCoroutine(TimeCoroutine(onEndTimer));
    }

    public void SetScore(int score)
    {
        this.score = score;

        if (scoreCoroutine != null)
            CoroutineHelper.StopCoroutine(scoreCoroutine);

        scoreCoroutine = CoroutineHelper.StartCoroutine(ScoreCoroutine(currScore));
    }

    private void DecreaseTime()
    {
        --second;

        if (second <= 0)
        {
            second = 59;
            --minute;
        }

        OnTimeChangedEvent?.Invoke();
    }

    private bool IsTimerEnd()
    {
        return minute == 0 && second == 0;
    }

    private IEnumerator TimeCoroutine(Action onEndTimer = null)
    {
        WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);

        while (!IsTimerEnd())
        {
            yield return waitForOneSecond;

            DecreaseTime();
        }

        onEndTimer?.Invoke();
    }

    private IEnumerator ScoreCoroutine(int currScore)
    {
        float time = 0f;

        while(time < SCORE_COROUTINE_TIME)
        {
            time += Time.deltaTime;

            this.currScore = (int)Mathf.Lerp(currScore, score, time / SCORE_COROUTINE_TIME);

            OnScoreChangedEvent?.Invoke(this.currScore);

            yield return null;
        }

        this.currScore = score;
        OnScoreChangedEvent?.Invoke(score);

        scoreCoroutine = null;
    }

    public override void Clear()
    {
        if (timeCoroutine != null)
            CoroutineHelper.StopCoroutine(timeCoroutine);

        if (scoreCoroutine != null)
            CoroutineHelper.StopCoroutine(scoreCoroutine);
    }
}
