using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


public class Shark : Creature
{
    public EMonsterType MonsterType { get; protected set; }
    [SerializeField]
    private bool IsStart = false;

    [SerializeField, ReadOnly] private JMonsterData data = null;
    [SerializeField] private float moveSpeed = 5f;
    
    private void Start() // 임시
    {
        SetInfo(0);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        this.gameObject.tag = Define.ETag.Monster.ToString();
        this.gameObject.layer = (int)ELayer.Monster;
        CreatureType = ECreatureType.Monster;
        this.transform.forward = new Vector3(0, 0, -1);

        moveStart();
        return true;
    }

    public override void SetInfo(int templateId)
    {
        MonsterType = (EMonsterType)templateId;

        data = Managers.Data.MonsterDict[(int)MonsterType];
    }

    public void moveStart()
    {
        IsStart = true;
        if (coSharkMove == null)
            coSharkMove = StartCoroutine(CoSharkMove());
    }

   

    Coroutine coSharkMove = null;

    protected IEnumerator CoSharkMove()
    {
        while (IsStart)
        {
            Debug.LogWarning("start");
            Debug.LogWarning(transform.forward);
            transform.Translate(transform.forward * -moveSpeed*Time.deltaTime);


            yield return null;
        }
        coSharkMove = null;
    }
    public override void OnCollisionTriggerEnter(Collider other)
    {
        base.OnCollisionTriggerEnter(other);

    }
}
