using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace CrossingBridge
{
    public class PlatformElement : InitBase
    {
        public event Action<ETeamType, bool> OnLandPlayer = null;
        
        [field: SerializeField, ReadOnly]
        public bool IsLandable { get; protected set; } = false;

        public void SetInfo(Action<ETeamType, bool> onLandPlayer, bool isLandable)
        {
            OnLandPlayer = onLandPlayer;
            this.IsLandable = isLandable;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.TryGetComponent(out Player player))
            {
                OnLandPlayer?.Invoke(player.TeamType, IsLandable);
            }
        }
    }
}