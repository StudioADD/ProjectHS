using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterType
{
    Shark = 0,
    BigShark,

    Max
}

public abstract class BaseMonster : Creature
{
    public EMonsterType MonsterType { get; protected set; }
    protected JMonsterData data { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        return true;
    }
    
    public override void SetInfo(int templateId)
    {
        MonsterType = (EMonsterType)templateId;
        data = Managers.Data.MonsterDict[templateId];
    }
}
