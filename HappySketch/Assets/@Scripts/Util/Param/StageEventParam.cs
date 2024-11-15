using System;
using UnityEngine;
using static Define;

public class StageEventParam { }

public class SharkAvoidanceParam : StageEventParam
{

}

public class CollectingCandyParam : StageEventParam
{

}

public class CrossingBridgeParam : StageEventParam
{
    public Func<int, bool, Vector3> GetJumpTargetPos;

    public CrossingBridgeParam(Func<int, bool, Vector3> getJumpTargetPos)
    {
        GetJumpTargetPos = getJumpTargetPos;
    }
}