using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : InitBase
{
    [SerializeField, ReadOnly] Collider Collider;
    public event Action<Collider> OnCollisionTriggerEnter = null;
    public event Action<Collider> OnCollisionTriggerExit = null;

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
        if (OnCollisionTriggerEnter != null)
            OnCollisionTriggerEnter.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if(OnCollisionTriggerExit != null)
            OnCollisionTriggerExit.Invoke(other);
    }
}
