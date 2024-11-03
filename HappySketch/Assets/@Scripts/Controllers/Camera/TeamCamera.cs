using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInfoData
{
    public float fieldOfView = 60f;
    public float targetDistance = 5f;
    public float cameraHeight = 0f;
    public float lookAtHeight = 0f;

    public CameraInfoData(float fieldOfView, float targetDistance, float cameraHeight, float lookAtHeight)
    {
        this.fieldOfView = fieldOfView;
        this.targetDistance = targetDistance;
        this.cameraHeight = cameraHeight;
        this.lookAtHeight = lookAtHeight;
    }
}

public class TeamCamera : InitBase
{
    [SerializeField, ReadOnly] protected Camera cam;
    [SerializeField, ReadOnly] protected BaseObject target = null;

    CameraInfoData cameraInfoData;

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

    public void SetInfo(CameraInfoData cameraInfoData)
    {
        this.cameraInfoData = cameraInfoData;
        cam.fieldOfView = cameraInfoData.fieldOfView;
    }

    public void SetTarget(BaseObject target) => this.target = target;

    private void FollowingTarget()
    {
        transform.position = target.transform.position + new Vector3(0, cameraInfoData.cameraHeight, cameraInfoData.targetDistance * -1);
        transform.LookAt(target.transform.position + new Vector3(0, cameraInfoData.lookAtHeight, 0));
    }
}
