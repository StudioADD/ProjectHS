using System;
using UnityEngine;
using static Define;

public class StageParam { }

public class SharkAvoidanceParam : StageParam
{

}

public class CollectingCandyParam : StageParam
{

}

public class CrossingBridgeParam : StageParam
{
    public Func<int, bool, Vector3> GetJumpTargetPos;
}



public class A
{
    public event Func<int, bool, Vector3> OnTest;
}

// 스테이지 전용 클래스가 있어야 함.
// 여기에 접근할 수 있는 방법도 필요해

// 스테이지 전용 클래스 -> Param이 같이 매핑된 데이터로 주고받을 수 있어야 함.
// StageParam -> Param에 묶어서 한번에 주고받고 함.
