using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public abstract class ModelBase
{
    public ETeamType TeamType { get; private set; }

    public ModelBase(ETeamType teamType)
    {
        TeamType = teamType;
    }
}
