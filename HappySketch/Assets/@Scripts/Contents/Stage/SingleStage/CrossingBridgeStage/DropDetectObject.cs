using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDetectObject : InitBase
{
    Collider Collider;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider = GetComponent<Collider>();
        Collider.isTrigger = false;

        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Player player))
        {
            player.OnDropDetect();
        }
    }
}
