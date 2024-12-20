using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeEffectPopup : UI_BasePopup
{
    [SerializeField] private Image fadeEffectImage;

    private Coroutine fadeEffectCoroutine = null;
    private Func<bool> fadeInEffectCondition = null;
    private Action onFadeOutCallBack = null;
    private Action onFadeInCallBack = null;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void OpenPopupUI()
    {
        base.OpenPopupUI();

        if (fadeEffectCoroutine == null)
        {
            fadeEffectCoroutine = StartCoroutine(IfadeOutInEffect(0.7f));
        }
        else
            Debug.Log("FadeEffect 중복");
    }

    public override void SetInfo(UIParam param)
    {
        if (param is UIFadeEffectParam fadeEffectParam)
        {
            this.fadeInEffectCondition = fadeEffectParam.fadeInEffectCondition;
            this.onFadeOutCallBack = fadeEffectParam.onFadeOutCallBack;
            this.onFadeInCallBack = fadeEffectParam.onFadeInCallBack;
        }
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();

        fadeInEffectCondition = null;
        onFadeOutCallBack = null;
        onFadeInCallBack = null;
    }

    public override void DeActivePopup()
    {
        if (fadeEffectCoroutine != null)
            return;

        base.DeActivePopup();
    }

    private IEnumerator IfadeOutInEffect(float fadeTime)
    {
        // FadeOut Effect
        fadeEffectImage.color = new Color(0, 0, 0, 0);
        Color tempColor = fadeEffectImage.color;

        while (tempColor.a < 0.99f)
        {
            tempColor.a += Time.deltaTime / fadeTime;
            fadeEffectImage.color = tempColor;

            yield return null;
        }

        tempColor.a = 1f;
        fadeEffectImage.color = tempColor;

        onFadeOutCallBack?.Invoke();

        // Wait Condition
        if (fadeInEffectCondition != null)
        {
            yield return new WaitUntil(fadeInEffectCondition);
            yield return new WaitForSeconds(1f); // 1초 대기 (임시)
        }

        // FadeIn Effect
        while (tempColor.a > 0.01f)
        {
            tempColor.a -= Time.deltaTime / fadeTime;
            fadeEffectImage.color = tempColor;

            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }

        onFadeInCallBack?.Invoke();

        fadeEffectImage.color = tempColor;
        fadeEffectCoroutine = null;
        ClosePopupUI();
    }
}