using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : BaseMonster
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

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
            // 플레이어에게 히트 이벤트 발생시켜야 함
        }
        else if(other.CompareTag(Util.EnumToString(Define.ETag.Player)))
        {

        }
    }
}
 