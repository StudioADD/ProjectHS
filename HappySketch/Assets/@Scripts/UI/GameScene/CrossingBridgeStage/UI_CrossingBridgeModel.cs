using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class UI_CrossingBridgeModel : UI_ModelBase
{
    private const float COLOR_TIME = 2f;

    public event UnityAction<Color> OnColorChangedEvent;
    private Coroutine colorCoroutine;

    public UI_CrossingBridgeModel() : base()
    {

    }

    public void SetColor(Color currColor, Color targetColor)
    {
        if (colorCoroutine != null)
            CoroutineHelper.StopCoroutine(colorCoroutine);

        colorCoroutine = CoroutineHelper.StartCoroutine(ColorCoroutine(currColor, targetColor, COLOR_TIME));
    }

    private IEnumerator ColorCoroutine(Color currColor, Color targetColor, float colorTime)
    {
        float time = 0f;

        while(time < colorTime)
        {
            time += Time.deltaTime;

            Color color = Color.Lerp(currColor, targetColor, time / colorTime);

            OnColorChangedEvent?.Invoke(color);

            yield return null;
        }

        OnColorChangedEvent?.Invoke(targetColor);
    }

    public override void Clear()
    {
        if (colorCoroutine != null)
            CoroutineHelper.StopCoroutine(colorCoroutine);
    }
}
