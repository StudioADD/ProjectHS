using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace CrossingBridge
{
    public class PlatformGroup : BasePlatformGroup
    {
        [SerializeField, ReadOnly] Transform leftPlatformTr;
        [SerializeField, ReadOnly] Transform rightPlatformTr;

        [SerializeField, ReadOnly] DynamicPlatform leftPlatform;
        [SerializeField, ReadOnly] DynamicPlatform rightPlatform;

        private void Reset()
        {
            leftPlatformTr = Util.FindChild<Transform>(gameObject, "LeftPlatformPoint", true);
            rightPlatformTr = Util.FindChild<Transform>(gameObject, "RightPlatformPoint", true);

            leftPlatform = Util.FindChild<DynamicPlatform>(gameObject, "LeftPlatformElement", true);
            rightPlatform = Util.FindChild<DynamicPlatform>(gameObject, "RightPlatformElement", true);
        }

        public override void SetInfo(Action<int, ETeamType> onLandPlayer, int platformId)
        {
            base.SetInfo(onLandPlayer, platformId);
            
            bool isLandable = (UnityEngine.Random.value > 0.5f);
            leftPlatform.SetInfo(OnLandPlayerCallBack, EDirection.Left, isLandable);
            rightPlatform.SetInfo(OnLandPlayerCallBack, EDirection.Right, !isLandable);
        }

        public override Vector3 GetPlatformPosition(ETeamType teamType, EDirection dir)
        {
            Vector3 offsetVec = new Vector3((teamType == ETeamType.Left) ? -1 : 1, 0, 0);

            if (teamType == ETeamType.Left)
                return leftPlatformTr.position + offsetVec;
            else if (teamType == ETeamType.Right)
                return rightPlatformTr.position + offsetVec;

#if DEBUG
            Debug.LogError("없는 타입");
#endif
            return Vector3.zero;
        }
    }
}
