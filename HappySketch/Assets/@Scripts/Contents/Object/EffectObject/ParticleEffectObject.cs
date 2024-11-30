using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectObject : BaseEffectObject
{
    [SerializeField, ReadOnly]
    protected List<ParticleSystem> particleList = new List<ParticleSystem>();

    protected virtual void Reset()
    {
        Transform[] myChildren = this.GetComponentsInChildren<Transform>();

        foreach (Transform child in myChildren)
        {
            if (child.TryGetComponent<ParticleSystem>(out ParticleSystem particle))
            {
                particleList.Add(particle);
            }
        }
    }
}
