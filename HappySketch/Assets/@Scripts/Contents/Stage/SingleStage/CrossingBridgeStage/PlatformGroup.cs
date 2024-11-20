using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace CrossingBridge
{
    public class PlatformGroup : InitBase
    {
        public event Action<int, ETeamType, bool> OnLandPlayer = null;

        [SerializeField, ReadOnly] PlatformElement leftPlatform;
        [SerializeField, ReadOnly] PlatformElement rightPlatform;
        [SerializeField, ReadOnly] int platformId;

        private void Reset()
        {
            leftPlatform = Util.FindChild<PlatformElement>(gameObject, "LeftPlatformElement", false);
            rightPlatform = Util.FindChild<PlatformElement>(gameObject, "RightPlatformElement", false);
        }

        public void SetInfo(Action<int, ETeamType, bool> onLandPlayer, int platformId)
        {
            OnLandPlayer = onLandPlayer;
            this.platformId = platformId;

            bool isLandable = (UnityEngine.Random.value > 0.5f);
            leftPlatform.SetInfo(OnLandPlayerCallBack, isLandable);
            leftPlatform.SetInfo(OnLandPlayerCallBack, !isLandable);
        }

        public void OnLandPlayerCallBack(ETeamType teamType, bool isLandable)
        {
            OnLandPlayer?.Invoke(platformId, teamType, isLandable);
        }

        public bool IsLandableLeft() { return leftPlatform.IsLandable; }
    }
}
