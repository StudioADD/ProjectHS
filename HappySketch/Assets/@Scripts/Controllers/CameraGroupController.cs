using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CameraGroupController : InitBase
{
    [SerializeField] TeamCamera leftCamera;      // Blue Team
    [SerializeField] TeamCamera rightCamera;     // Red Team

    [SerializeField] Vector3 deltaPosVec;

    protected virtual void Reset()
    {
        leftCamera = Util.FindChild<TeamCamera>(gameObject, "LeftCamera");
        rightCamera = Util.FindChild<TeamCamera>(gameObject, "RightCamera");
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo()
    {
        // 현재 스테이지 Id를 가져와서 각 카메라에 로드경로를 부여

    }

    public void SetTarget(BaseObject target, ETeamType type)
    {
        switch (type)
        {
            case ETeamType.Left:
                leftCamera.SetTarget(target);
                break;
            case ETeamType.Right:
                rightCamera.SetTarget(target);
                break;
        }
    }
}
