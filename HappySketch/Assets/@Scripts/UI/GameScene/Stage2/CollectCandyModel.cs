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

    [SerializeField, ReadOnly]
    private int second = 0;

    [SerializeField, ReadOnly]
    private int minute = 0;

    public event UnityAction OnTimeChangedEvent;
    private Coroutine timeCoroutine;

    int score;
    int currScore;

    public event UnityAction<int> OnScoreChangedEvent;
    private Coroutine scoreCoroutine;

    public CollectCandyModel() : base()
    {

    }

    public string GetFormattedTime()
    {
        return string.Format("{0:D2}:{1:D2}", minute, second);
    }

    public void StartTimer()
    {
        timeCoroutine = CoroutineHelper.StartCoroutine(TimeCoroutine());
    }

    public void SetScore(int score)
    {
        this.score = score;

        if (scoreCoroutine != null)
            CoroutineHelper.StopCoroutine(scoreCoroutine);

        scoreCoroutine = CoroutineHelper.StartCoroutine(ScoreCoroutine(currScore));
    }

    private void IncreaseTime()
    {
        ++second;
        
        if(second == 60)
        {
            second = 0;
            ++minute;
        }

        OnTimeChangedEvent?.Invoke();
    }

    private IEnumerator TimeCoroutine()
    {
        WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);

        while (true)
        {
            yield return waitForOneSecond;

            IncreaseTime();
        }
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
