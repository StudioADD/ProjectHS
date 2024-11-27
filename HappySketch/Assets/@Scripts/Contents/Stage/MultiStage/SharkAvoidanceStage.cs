using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SharkAvoidanceStage : MultiStage
{
    // 임시 테스트 코드
    [SerializeField] float StartMonsterSpawnDelay = 5f;
    [SerializeField] float EndMonsterSpawnDelay = 7f;

    enum EStageSection
    {
        None,
        Level1,
        Level2
    }

    [SerializeField, ReadOnly]
    protected FinishLineObject finishLineObject;

    [field: SerializeField, ReadOnly]
    List<SpawnPointObject> spawnPointList = new List<SpawnPointObject>();

    [field: SerializeField, ReadOnly] 
    SharkAvoidanceParam sharkAvoidanceParam = null;

    protected override void Reset()
    {
        base.Reset();

        finishLineObject = Util.FindChild<FinishLineObject>(gameObject, "FinishLineObject", false);

        Transform[] children = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.TryGetComponent<SpawnPointObject>(out SpawnPointObject spawnPoint))
                spawnPointList.Add(spawnPoint);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StageType = EStageType.SharkAvoidance;

        return true;
    }

    public override void SetInfo(Player player = null)
    {
        base.SetInfo(player);

        sharkAvoidanceParam = new SharkAvoidanceParam(TeamType, 1f, 0);
    }

    public override void StartStage()
    {
        base.StartStage();

        if(coSpawnMonster != null)
            StopCoroutine(coSpawnMonster);

        coSpawnMonster = StartCoroutine(CoSpawnMonster());
    }

    public override void ConnectEvents(Action<Define.ETeamType> onEndGameCallBack)
    {
        if (finishLineObject != null)
        {
            finishLineObject.OnArriveFinishLine -= onEndGameCallBack;
            finishLineObject.OnArriveFinishLine += onEndGameCallBack;
        }
        else
            Debug.LogWarning($"FinishLineObject is Null!!");
    }

    Coroutine coSpawnMonster = null;
    private IEnumerator CoSpawnMonster()
    {
        while (Managers.Game.IsGamePlay)
        {
            EStageSection stageSection = CheckStageSection();
            
            // 테스트(임시)
            sharkAvoidanceParam.BoosterCount++;
            if (sharkAvoidanceParam.BoosterCount == 4)
                sharkAvoidanceParam.BoosterCount = 0;
            OnReceiveStageParamCallBack(sharkAvoidanceParam);

            switch (stageSection)
            {
                case EStageSection.Level1:
                    SpawnMonster(UnityEngine.Random.Range(1, 3));
                    break;
                case EStageSection.Level2:
                    SpawnMonster(UnityEngine.Random.Range(2, 4));
                    break;
            }

            if (EndMonsterSpawnDelay < StartMonsterSpawnDelay)
                EndMonsterSpawnDelay = StartMonsterSpawnDelay;

            float delayTime = UnityEngine.Random.Range(StartMonsterSpawnDelay, EndMonsterSpawnDelay);
            Debug.Log($"{delayTime}초 뒤에 몬스터 생성");
            yield return new WaitForSeconds(delayTime); // 임시
        }

        coSpawnMonster = null;
    }

    protected override IEnumerator CoReceiveStageParam()
    {
        while(true)
        {

            yield return new WaitForSeconds(0.5f);
        }

        coReceiveStageParam = null;
    }

    private EStageSection CheckStageSection()
    {
        float stageLength = Mathf.Abs(finishLineObject.transform.position.z - playerStartPoint.position.z);
        float goalLength = Mathf.Abs(finishLineObject.transform.position.z - player.transform.position.z);

        sharkAvoidanceParam.CurrDisRatio = goalLength / stageLength;
        int goalPercent = (int)(sharkAvoidanceParam.CurrDisRatio * 100); // 0 ~ 100
        sharkAvoidanceParam.CurrDisRatio = Mathf.Abs(sharkAvoidanceParam.CurrDisRatio - 1);

        if (goalPercent > 99) // 테스트
            return EStageSection.Level1; // EStageSection.None;
        else if (goalPercent > 50)
            return EStageSection.Level1;
        else
            return EStageSection.Level2;
    }

    private void SpawnMonster(int spawnCount)
    {
        switch(spawnCount)
        {
            case 1:
                {
                    int spawnPointNum = UnityEngine.Random.Range(0, 4) * 2;
                    MonsterCreater.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointList[spawnPointNum].transform.position);
                }
                break;
            case 2:
                {
                    bool isBigShark = (UnityEngine.Random.value > 0.5f);
                    if(isBigShark)
                    {
                        int spawnPointNum = UnityEngine.Random.Range(0, 3) * 2 + 1;
                        MonsterCreater.SpawnMonster<Shark>(EMonsterType.BigShark, spawnPointList[spawnPointNum].transform.position);
                    }
                    else
                    {
                        int spawnPointNum1 = UnityEngine.Random.Range(0, 4) * 2;
                        int spawnPointNum2 = spawnPointNum1 + (UnityEngine.Random.Range(1, 4) * 2);
                        if(spawnPointNum2 >= spawnPointList.Count)
                            spawnPointNum2 -= spawnPointList.Count + 1;

                        MonsterCreater.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointList[spawnPointNum1].transform.position);
                        MonsterCreater.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointList[spawnPointNum2].transform.position);
                    }
                }
                break;
            case 3: 
                {
                    bool isBigShark = (UnityEngine.Random.value > 0.5f);
                    if (!isBigShark)
                    {
                        int spawnPointNum1 = UnityEngine.Random.Range(0, 3) * 2 + 1;
                        int spawnPointNum2 = spawnPointNum1 + 1 + UnityEngine.Random.Range(1, 3) * 2;
                        if (spawnPointNum2 >= spawnPointList.Count)
                            spawnPointNum2 -= spawnPointList.Count + 1;

                        MonsterCreater.SpawnMonster<Shark>(EMonsterType.BigShark, spawnPointList[spawnPointNum1].transform.position);
                        MonsterCreater.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointList[spawnPointNum2].transform.position);
                    }
                    else
                    {
                        int spawnExceptionNum = UnityEngine.Random.Range(0, 4) * 2;

                        for(int i = 1; i <= 3; i++)
                        {
                            int spawnPointNum = spawnExceptionNum + (i * 2);
                            if (spawnPointNum >= spawnPointList.Count)
                                spawnPointNum -= spawnPointList.Count + 1;

                            MonsterCreater.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointList[spawnPointNum].transform.position);
                        }
                    }
                }
                break;
        }

    }
}