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
            leftPlatform = Util.FindChild<DynamicPlatform>(gameObject, "LeftPlatformElement", false);
            rightPlatform = Util.FindChild<DynamicPlatform>(gameObject, "RightPlatformElement", false);
        }

        public override void SetInfo(Action<int, ETeamType, EDirection> onLandPlayer, int platformId)
        {
            base.SetInfo(onLandPlayer, platformId);
            
            bool isLandable = (UnityEngine.Random.value > 0.5f);
            leftPlatform.SetInfo(OnLandPlayerCallBack, EDirection.Left, isLandable);
            leftPlatform.SetInfo(OnLandPlayerCallBack, EDirection.Right, !isLandable);
        }
    }
}
