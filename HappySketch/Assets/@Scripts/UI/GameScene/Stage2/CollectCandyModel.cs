using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Define;

public class CollectCandyModel : ModelBase
{
    public ETeamType TeamType { get; private set; }

    [SerializeField, ReadOnly]
    private int second = 0;

    [SerializeField, ReadOnly]
    private int minute = 0;

    public event UnityAction TimeChangedEvent;

    private WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);

    public void SetTeamType(ETeamType teamType)
    {
        TeamType = teamType;
    }

    public string GetFormattedTime()
    {
        return string.Format("{0:D2}:{1:D2}", minute, second);
    }

    public void StartTimer()
    {
        StartCoroutine(TimeCoroutine());
    }

    private void IncreaseTime()
    {
        ++second;
        
        if(second == 60)
        {
            second = 0;
            ++minute;
        }

        TimeChangedEvent.Invoke();
    }

    private IEnumerator TimeCoroutine()
    {
        while (true)
        {
            yield return waitForOneSecond;

            IncreaseTime();
        }
    }
}
