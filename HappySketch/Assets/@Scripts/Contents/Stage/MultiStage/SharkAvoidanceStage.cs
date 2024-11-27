using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SharkAvoidanceStage : MultiStage
{
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

            switch(stageSection)
            {
                case EStageSection.Level1:
                    SpawnMonster(UnityEngine.Random.Range(1, 3));
                    break;
                case EStageSection.Level2:
                    SpawnMonster(UnityEngine.Random.Range(2, 4));
                    break;
            }

            
            yield return new WaitForSeconds(5f); // 임시
        }

        coSpawnMonster = null;
    }

    private EStageSection CheckStageSection()
    {
        float stageLength = Mathf.Abs(finishLineObject.transform.position.z - playerStartPoint.position.z);
        float goalLength = Mathf.Abs(finishLineObject.transform.position.z - player.transform.position.z);

        int goalPercent = (int)(goalLength / stageLength * 100); // 0 ~ 100

        if (goalPercent > 90) // 테스트
            return EStageSection.Level1; // EStageSection.None;
        else if (goalPercent > 45)
            return EStageSection.Level1;
        else // 55 ~ 100
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
                            spawnPointNum2 -= spawnPointList.Count;

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
                            spawnPointNum2 -= spawnPointList.Count;

                        MonsterCreater.SpawnMonster<Shark>(EMonsterType.BigShark, spawnPointList[spawnPointNum1].transform.position);
                        MonsterCreater.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointList[spawnPointNum2].transform.position);
                    }
                    else
                    {
                        int spawnExceptionNum = UnityEngine.Random.Range(0, 4) * 2;

                        for(int i = 0; i < 3; i++)
                        {
                            int spawnPointNum = spawnExceptionNum + (i * 2);
                            if (spawnPointNum >= spawnPointList.Count)
                                spawnPointNum -= spawnPointList.Count - 1;

                            MonsterCreater.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointList[spawnPointNum].transform.position);
                        }
                    }
                }
                break;
        }

    }
}