using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace CrossingBridge
{
    public class PlatformGroup : BasePlatformGroup
    {
        [SerializeField, ReadOnly] DynamicPlatform leftPlatform;
        [SerializeField, ReadOnly] DynamicPlatform rightPlatform;

        private void Reset()
        {
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

            if (dir == EDirection.Left)
                return leftPlatform.transform.position + offsetVec;
            else if (dir == EDirection.Right)
                return rightPlatform.transform.position + offsetVec;

#if DEBUG
            Debug.LogError("없는 타입");
#endif
            return Vector3.zero;
        }
    }
}
