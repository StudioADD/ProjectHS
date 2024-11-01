using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CameraGroupController : InitBase
{
    [SerializeField] private TeamCamera leftCamera;      // Blue Team
    [SerializeField] private TeamCamera rightCamera;     // Red Team

    [SerializeField] Vector3 deltaPosVec;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo()
    {
        // 각 카메라에 같은 세팅을 부여
        leftCamera.SetInfo(deltaPosVec);
        rightCamera.SetInfo(deltaPosVec);
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
