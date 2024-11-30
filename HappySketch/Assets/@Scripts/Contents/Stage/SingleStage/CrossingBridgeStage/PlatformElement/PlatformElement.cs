using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace CrossingBridge
{
    public class DynamicPlatform : InitBase
    {
        public event Action<ETeamType, EDirection> OnLandPlayer = null;
        [SerializeField, ReadOnly] EDirection dir;
        [field: SerializeField, ReadOnly] public bool IsLandable { get; protected set; }

        Animation anim;
        
        public void SetInfo(Action<ETeamType, EDirection> onLandPlayer, EDirection dir, bool isLandable)
        {
            OnLandPlayer = onLandPlayer;
            this.dir = dir;
            IsLandable = isLandable;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.TryGetComponent(out Player player))
            {
                OnLandPlayer?.Invoke(player.TeamType, dir);
                
                if(IsLandable == false)
                {
                    anim.Play(STRING_EFFECT);
                }
            }
        }
    }
}