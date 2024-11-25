using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : BaseMonster
{
    public enum ESharkState
    {
        Idle,
        Move
    }

    private ESharkState _state  = ESharkState.Idle;
    public ESharkState State 
    { 
        get { return _state; } 
        private set
        {
            if (_state == value)
                return;

            _state = value;
            animator.Play(Util.EnumToString(value));

            if(value == ESharkState.Move)
            {
                Test();
            }
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);

        switch((EMonsterType)templateId)
        {
            case EMonsterType.Shark:

                break;
            case EMonsterType.BigShark:
                
                break;
            default:
                Debug.LogWarning($"없는 타입 : {(EMonsterType)templateId}");
                break;
        }
    }

    public void Test()
    {
        State = ESharkState.Move;
        SetRigidVelocityZ(data.MoveSpeed * -1);
        if(this.gameObject.activeSelf)
            coDestroyCheck = StartCoroutine(CoDestroyCheck());
    }

    Coroutine coDestroyCheck = null;
    private IEnumerator CoDestroyCheck()
    {
        while (true)
        {
            if (this.transform.position.z < -150)
                break;

            yield return new WaitForSeconds(1);
        }

        coDestroyCheck = null;
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
 