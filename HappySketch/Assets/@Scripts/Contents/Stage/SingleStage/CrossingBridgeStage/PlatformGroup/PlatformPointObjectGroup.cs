using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace CrossingBridge
{
    public class PlatformPointObjectGroup : BasePlatformGroup
    {
        [SerializeField, ReadOnly] PlatformElement platformElement;

        private void Reset()
        {
            platformElement = Util.FindChild<PlatformElement>(gameObject);
        }

        public override void SetInfo(Action<int, Define.ETeamType> onLandPlayer, int platformId)
        {
            base.SetInfo(onLandPlayer, platformId);

            platformElement.SetInfo(OnLandPlayerCallBack);
        }

        public override Vector3 GetPlatformPosition(ETeamType teamType, EDirection dir = EDirection.Left)
        {
#if DEBUG
            Debug.LogError("호출되지 않아야 할 메서드가 호출됨");
#endif
            return platformElement.transform.position;
        }
    }
}
    
