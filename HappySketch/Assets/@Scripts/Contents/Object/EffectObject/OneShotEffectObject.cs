using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotEffectObject : EffectObject
{
    [SerializeField, ReadOnly] float maxDuration = 0;

    protected override void Reset()
    {
        base.Reset();

        foreach(ParticleSystem particle in particleList)
        {
            float time = (particle.main.duration + particle.main.startDelayMultiplier);
            maxDuration = Mathf.Max(maxDuration, time);
        }
    }

    public override void SetInfo()
    {
        base.SetInfo();

        if(coWaitEndEffect == null)
            coWaitEndEffect = StartCoroutine(CoWaitEndEffect());
    }

    Coroutine coWaitEndEffect = null;
    IEnumerator CoWaitEndEffect()
    {
        yield return new WaitForSeconds(maxDuration);
        Managers.Resource.Destroy(gameObject);
    }
}
