using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace CrossingBridge
{
    public class PlatformElement : InitBase
    {
        Action<ETeamType> onLandPlayer = null;

        public void SetInfo(Action<ETeamType> onLandPlayer)
        {
            this.onLandPlayer = onLandPlayer;
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out Player player))
            {
                onLandPlayer?.Invoke(player.TeamType);
            }
        }
    }
}