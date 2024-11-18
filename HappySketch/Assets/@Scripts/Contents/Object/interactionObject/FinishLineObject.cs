using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FinishLineObject : BaseObject
{
    public event Action<ETeamType> OnArriveFinishLine;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out Player player))
        {
            OnArriveFinishLine?.Invoke(player.TeamType);
        }
    }
}
