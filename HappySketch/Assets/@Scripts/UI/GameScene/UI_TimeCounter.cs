using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_TimeCounter : InitBase
{
    public event UnityAction TimeEndEvent;

    private WaitForSeconds waitForSecond = new WaitForSeconds(1f);
    private int time;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetTimer(int time)
    {
        this.time = time;
    }

    private IEnumerator TimeCoroutine()
    {
        while(time > 0)
        {
            yield return waitForSecond;
            --time;
        }
    }
}
