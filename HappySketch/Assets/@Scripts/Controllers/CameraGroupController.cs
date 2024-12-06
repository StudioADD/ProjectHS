using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UnityEngine.GraphicsBuffer;

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

    public void SetInfo(EStageType stageType)
    {
        leftCamera.SetInfo(stageType);
        rightCamera.SetInfo(stageType);
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

    public Camera GetTeamCamera(ETeamType type)
    {
        return (type == ETeamType.Left ? leftCamera.GetTeamCamera() : rightCamera.GetTeamCamera());
    }
}
