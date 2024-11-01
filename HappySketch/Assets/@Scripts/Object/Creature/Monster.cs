using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterType
{
    SmallShark = 0,
    BigShark = 1,

    Max
}

public class Monster : Creature
{
    public EMonsterType MonsterType { get; protected set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        this.gameObject.tag = Define.ETag.Monster.ToString();
        CreatureType = ECreatureType.Monster;

        return true;
    }

    public override void SetInfo(int templateId)
    {
        MonsterType = (EMonsterType)templateId;

        // MonsterType에 따른 정보를 가지고 옴
    }
}
