using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamCamera : InitBase
{
    [SerializeField, ReadOnly] Camera cam;
    [SerializeField, ReadOnly] BaseObject target = null;
    [SerializeField, ReadOnly] Vector3 deltaPosVec;

    private void LateUpdate()
    {
        if (target != null)
        {
            FollowingTarget();
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        cam = GetComponent<Camera>();
        // 카메라 공용 값 세팅이 필요함

        return true;
    }

    public void SetInfo(Vector3 deltaPosVec)
    {
        this.deltaPosVec = deltaPosVec;
    }

    public void SetTarget(BaseObject target) => this.target = target;

    private void FollowingTarget()
    {
        transform.position = target.transform.position + deltaPosVec;
        transform.LookAt(target.transform);
    }
}
