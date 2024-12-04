using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static Define; 

public class CollectCandyModel : ModelBase
{
    private class Score
    {
        public int currScore;
        public int score;

        public Score(int currScore = 0, int score = 0)
        {
            this.currScore = currScore;
            this.score = score;
        }
    }

    private const float SCORE_COROUTINE_TIME = 1;

    private int second = 0;
    private int minute = 0;

    public event UnityAction OnTimeChanged;
    private Coroutine timeCoroutine;

    private Score leftScore;
    private Score rightScore;

    public event Action<int> OnLeftScoreChanged;
    public event Action<int> OnRightScoreChanged;

    private Coroutine leftScoreCoroutine;
    private Coroutine rightScoreCoroutine;

    public CollectCandyModel() : base()
    {
        
    }

    public Vector3 GetUIPos(Camera camera, Vector3 worldPos)
    {
        camera.WorldToViewportPoint(worldPos);
        return Vector2.zero;
    }

    public string GetFormattedTime()
    {
        return string.Format("{0:D2}:{1:D2}", minute, second);
    }

    public void StartTimer(int seconds, Action onEndTimer = null)
    {
        minute = seconds / 60;
        second = seconds % 60;

        OnTimeChanged?.Invoke();

        if (timeCoroutine != null)
            CoroutineHelper.StopCoroutine(timeCoroutine);

        timeCoroutine = CoroutineHelper.StartCoroutine(TimeCoroutine(onEndTimer));
    }

    public void SetLeftScore(int score)
    {
        leftScore.score = score;

        if (leftScoreCoroutine != null)
            CoroutineHelper.StopCoroutine(leftScoreCoroutine);

        leftScoreCoroutine = CoroutineHelper.StartCoroutine(ScoreCoroutine(leftScore.currScore, leftScore, OnLeftScoreChanged));
    }

    public void SetRightScore(int score)
    {
        rightScore.score = score;

        if (rightScoreCoroutine != null)
            CoroutineHelper.StopCoroutine(rightScoreCoroutine);

        CoroutineHelper.StartCoroutine(ScoreCoroutine(rightScore.currScore, rightScore, OnRightScoreChanged));
    }

    private void DecreaseTime()
    {
        --second;

        if (second < 0)
        {
            second = 59;
            --minute;
        }

        OnTimeChanged?.Invoke();
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

    private IEnumerator ScoreCoroutine(int startScore, Score score, Action<int> onScoreChanged)
    {
        float time = 0f;

        while(time < SCORE_COROUTINE_TIME)
        {
            time += Time.deltaTime;

            score.currScore = (int)Mathf.Lerp(startScore, score.score, time / SCORE_COROUTINE_TIME);

            onScoreChanged?.Invoke(score.currScore);

            yield return null;
        }

        score.currScore = score.score;
        onScoreChanged?.Invoke(score.score);

        leftScoreCoroutine = null;
    }

    public override void Clear()
    {
        if (timeCoroutine != null)
            CoroutineHelper.StopCoroutine(timeCoroutine);

        if (leftScoreCoroutine != null)
            CoroutineHelper.StopCoroutine(leftScoreCoroutine);

        if (rightScoreCoroutine != null)
            CoroutineHelper.StopCoroutine(rightScoreCoroutine);
    }
}
