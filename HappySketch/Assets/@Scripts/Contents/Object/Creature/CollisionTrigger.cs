using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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
        Collider.excludeLayers += 1 << (int)ELayer.Default;
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

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }


}
