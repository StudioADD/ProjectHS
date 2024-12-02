using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectObject : ParticleEffectObject
{
    ParticleSystem particle;

    public override void SetInfo()
    {
        base.SetInfo();
        particle = GetComponent<ParticleSystem>();
    }

    public void PlayEffect()
    {
        foreach (ParticleSystem item in particleList)
        {
            item?.Play();
        }
    }
    public void StopEffect()
    {
        foreach (ParticleSystem item in particleList)
        {
            item?.Stop();
        }

    }

    public bool GetIsPlay()
    {
        foreach (ParticleSystem item in particleList)
        {
            if (item.isPlaying)
                return true;
        }
        return false;
    }

    public void SetDuration(float duration, float speed)
    {
        foreach (ParticleSystem item in particleList)
        {
            var main = item.main;
            main.duration = duration;

            main.simulationSpeed = speed;
        }

    }
}
