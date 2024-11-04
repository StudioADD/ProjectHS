using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : InitBase
{
    [SerializeField, ReadOnly] Collider Collider;
    public event Action<Collider> OnCollisionTiggerEnter = null;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider = GetComponent<Collider>();
        Collider.isTrigger = true;
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnCollisionTiggerEnter != null)
            OnCollisionTiggerEnter.Invoke(other);
    }
}
