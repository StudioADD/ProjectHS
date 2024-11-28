using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameStartCounter : UI_BaseObject
{
    [SerializeField, ReadOnly] UI_ImageEffect[] leftTime = new UI_ImageEffect[3];
    [SerializeField, ReadOnly] UI_ImageEffect[] rightTime = new UI_ImageEffect[3];

    [SerializeField, ReadOnly] UI_ImageEffect leftGoImage;
    [SerializeField, ReadOnly] UI_ImageEffect rightGoImage;

    private WaitForSeconds waitForSecond = new WaitForSeconds(1f);

    private void Reset()
    {
        leftTime[0] = Util.FindChild<UI_ImageEffect>(transform.GetChild(0).gameObject, "Img_3_Left", false);
        leftTime[1] = Util.FindChild<UI_ImageEffect>(transform.GetChild(0).gameObject, "Img_2_Left", false);
        leftTime[2] = Util.FindChild<UI_ImageEffect>(transform.GetChild(0).gameObject, "Img_1_Left", false);

        rightTime[0] = Util.FindChild<UI_ImageEffect>(transform.GetChild(1).gameObject, "Img_3_Right", false);
        rightTime[1] = Util.FindChild<UI_ImageEffect>(transform.GetChild(1).gameObject, "Img_2_Right", false);
        rightTime[2] = Util.FindChild<UI_ImageEffect>(transform.GetChild(1).gameObject, "Img_1_Right", false);

        leftGoImage = Util.FindChild<UI_ImageEffect>(gameObject, "Img_Go_Left");
        rightGoImage = Util.FindChild<UI_ImageEffect>(gameObject, "Img_Go_Right");
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
                // param으로 time이 전달 되지만, Text가 아닌 Image로 변경했기 때문에, 3초로 고정한다!
                coGameStartCounter = StartCoroutine(CoGameStartCounter(3, gameStartCounterParam.OnEndCount));
            }
            else
            {
                Debug.LogWarning("타이머 중복 호출로 인해 타이머 변경 및 콜백 무시");
            }
        }
    }

    Coroutine coGameStartCounter = null;
    private IEnumerator CoGameStartCounter(int time, Action onEndCount = null)
    {
        for(int i = 0; i < 3; ++i)
        {
            leftTime[i].OpenImageEffectUI();
            rightTime[i].OpenImageEffectUI();

            yield return waitForSecond;

            leftTime[i].CloseTextEffectUI();
            rightTime[i].CloseTextEffectUI();
        }

        leftGoImage.OpenImageEffectUI();
        rightGoImage.OpenImageEffectUI();

        yield return waitForSecond;

        leftGoImage.CloseTextEffectUI();
        rightGoImage.CloseTextEffectUI();

        onEndCount?.Invoke();
        coGameStartCounter = null;
        Managers.Resource.Destroy(gameObject);
    }
}
