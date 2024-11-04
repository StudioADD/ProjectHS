using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public enum EMonsterType
{
    TestMonster = 0,
    SmallShark,
    BigShark,

    Max
}

public class Monster : Creature
{
    public EMonsterType MonsterType { get; protected set; }

    [SerializeField, ReadOnly] private JMonsterData data = null;

    private void Start()
    {
        SetInfo(0); // 임시
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        this.gameObject.tag = Define.ETag.Monster.ToString();
        this.gameObject.layer = (int)ELayer.Monster;
        CreatureType = ECreatureType.Monster;

        return true;
    }

    public override void SetInfo(int templateId)
    {
        MonsterType = (EMonsterType)templateId;

        data = Managers.Data.MonsterDict[(int)MonsterType];
    }

    private void Update()
    {
        if(data != null)
            SetRigidVelocityZ(data.MoveSpeed * -1f * Time.deltaTime);
    }
}
