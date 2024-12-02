using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : BaseMonster
{
    Animator anim;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        anim = GetComponent<Animator>();

        return true;
    }

    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);

        SetRigidVelocityZ(data.MoveSpeed * -1);
        StartCoroutine(CoDestroyCheck());
    }

    private IEnumerator CoDestroyCheck()
    {
        while (true)
        {
            if (this.transform.position.z < -150)
                break;

            yield return new WaitForSeconds(1);
        }

        Managers.Resource.Destroy(this.gameObject);
    }

    public override void OnCollisionTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player player))
        {
            anim.Play((player.IsBoosterState) ? Define.STRING_Hit : Define.STRING_ATTACK);

            player.OnHit(data.SternTime);
        }
    }
}
 