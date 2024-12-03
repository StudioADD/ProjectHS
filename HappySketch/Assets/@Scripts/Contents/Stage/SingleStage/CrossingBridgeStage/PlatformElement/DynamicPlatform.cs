using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace CrossingBridge
{
    public class DynamicPlatform : InitBase
    {
        [field: SerializeField, ReadOnly] public bool IsLandable { get; protected set; }

        Action<ETeamType> onLandPlayer = null;
        Animator anim;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            anim = GetComponent<Animator>();

            return true;
        }

        public void SetInfo(Action<ETeamType> onLandPlayer, EDirection dir, bool isLandable)
        {
            this.onLandPlayer = onLandPlayer;
            IsLandable = isLandable;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.TryGetComponent(out Player player))
            {
                if(IsLandable == true)
                    onLandPlayer?.Invoke(player.TeamType);
                else
                    anim.SetTrigger(STRING_EFFECTTRIGGER);
            }
        }
    }
}