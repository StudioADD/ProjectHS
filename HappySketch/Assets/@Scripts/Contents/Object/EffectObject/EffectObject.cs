using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public enum EEffectType
{
    ItemGainEffect = 0,
}

public class EffectObject : BaseObject
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

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        this.ObjectType = EObjectType.Effect;

        return true;
    }

    public virtual void SetInfo()
    {
        if(particleList.Count == 0)
        {
            Debug.LogError("파티클 이펙트가 아니거나, 세팅되지 않았습니다. 대응이 필요합니다.");
            return;
        }


    }

}