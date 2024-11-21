using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameStartCounter : UI_BaseObject
{
    [SerializeField, ReadOnly] UI_TextEffect leftTimerText;
    [SerializeField, ReadOnly] UI_TextEffect rightTimerText;

    [SerializeField, ReadOnly] UI_ImageEffect leftGoImage;
    [SerializeField, ReadOnly] UI_ImageEffect rightGoImage;

    private WaitForSeconds waitForSecond = new WaitForSeconds(1f);

    int _currTime = 0;
    public int CurrTIme 
    {
        protected set 
        { 
            _currTime = value;
            leftTimerText.OpenTextEffectUI(_currTime.ToString());
            rightTimerText.OpenTextEffectUI(_currTime.ToString());
        }
        get { return _currTime; }
    }

    private void Reset()
    {
        leftTimerText = Util.FindChild<UI_TextEffect>(gameObject, "LeftTimerText");
        rightTimerText = Util.FindChild<UI_TextEffect>(gameObject, "RightTimerText");

        leftGoImage = Util.FindChild<UI_ImageEffect>(gameObject, "LeftGoImage");
        rightGoImage = Util.FindChild<UI_ImageEffect>(gameObject, "RightGoImage");
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void SetInfo(UIParam param = null)
    {
        base.SetInfo(param);

        if(param is UIGameStartCounterParam gameStartCounterParam)
        {
            if (coGameStartCounter == null)
            {
                coGameStartCounter = StartCoroutine(CoGameStartCounter(gameStartCounterParam.Time, gameStartCounterParam.OnEndCount));
            }
            else
            {
                Debug.LogWarning("타이머 중복 호출로 인해 타이머 변경 및 콜백 무시");
                CurrTIme = gameStartCounterParam.Time;
            }
        }
    }

    Coroutine coGameStartCounter = null;
    private IEnumerator CoGameStartCounter(int time, Action onEndCount = null)
    {
        CurrTIme = time;
        yield return waitForSecond;

        while (CurrTIme > 1)
        {
            CurrTIme -= 1;
            yield return waitForSecond;
        }

        leftTimerText.CloseTextEffectUI();
        rightTimerText.CloseTextEffectUI();

        leftGoImage.OpenImageEffectUI();
        rightGoImage.OpenImageEffectUI();

        yield return waitForSecond;

        onEndCount?.Invoke();
        coGameStartCounter = null;
        Managers.Resource.Destroy(gameObject);
    }
}
