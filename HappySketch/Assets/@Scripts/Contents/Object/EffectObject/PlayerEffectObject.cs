using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectObject : EffectObject
{
 
    public override void SetInfo()
    {
        base.SetInfo();
    }

    public void SetFalse()
    {
        this.gameObject.SetActive(false);
    }

    public void SetTrue()
    {
        this.gameObject.SetActive(true);
    }

    public void SetDuration(float Duration)
    {
        foreach (ParticleSystem item in particleList)
        {
            var main = item.main;
            main.duration = Duration;
        }
    }
}
