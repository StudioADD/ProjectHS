using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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
            if(player.IsBoosterState)
            {
                anim.Play(STRING_Hit);
                Managers.Sound.PlaySfx(ESfxSoundType.SharkHit);
            }
            else
            {
                anim.Play(STRING_ATTACK);
                Managers.Sound.PlaySfx(ESfxSoundType.PlayerHit);
            }

            player.OnHit(data.SternTime);
        }
    }
}
 