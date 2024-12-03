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

    [SerializeField, ReadOnly] protected FinishLineObject finishLineObject;

    [field: SerializeField, ReadOnly] List<SpawnPointObject> spawnPointList = new List<SpawnPointObject>();
    [field: SerializeField, ReadOnly]  SharkAvoidanceParam stageParam = null;

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

        stageParam = new SharkAvoidanceParam(TeamType, 0, 0);
        SpawnItems();
    }

    public override void StartStage()
    {
        base.StartStage();

        if(coSpawnMonster != null)
            StopCoroutine(coSpawnMonster);

        if (coReceiveStageParam != null)
            StopCoroutine(coReceiveStageParam);

        coSpawnMonster = StartCoroutine(CoSpawnMonster());
        coReceiveStageParam = StartCoroutine(CoReceiveStageParam());
    }

    public override void EndStage(ETeamType winnerTeam)
    {
        base.EndStage(winnerTeam);

        if (coSpawnMonster != null)
            StopCoroutine(coSpawnMonster);

        if (coReceiveStageParam != null)
            StopCoroutine(coReceiveStageParam);
    }

    public override void ConnectEvents(Action<Define.ETeamType> onEndGameCallBack)
    {
        player.ConnectSharkAvoidanceStage(OnAddBoosterItem, OnUseBoosterItem);

        if (finishLineObject != null)
        {
            finishLineObject.OnArriveFinishLine -= onEndGameCallBack;
            finishLineObject.OnArriveFinishLine += onEndGameCallBack;
        }
        else
            Debug.LogWarning($"FinishLineObject is Null!!");
    }

    public void OnAddBoosterItem()
    {
        stageParam.BoosterCount++;

        if (stageParam.BoosterCount > 3)
            stageParam.BoosterCount = 3;

        OnReceiveStageParamCallBack(stageParam);
    }

    public bool OnUseBoosterItem()
    {
        if(stageParam.BoosterCount == 3)
        {
            stageParam.BoosterCount = 0;
            OnReceiveStageParamCallBack(stageParam);
            return true;
        }

        OnReceiveStageParamCallBack(stageParam);
        return false;
    }

    Coroutine coSpawnMonster = null;
    private IEnumerator CoSpawnMonster()
    {
        while (Managers.Game.IsGamePlay)
        {
            EStageSection stageSection = CheckStageSection();
            
            switch (stageSection)
            {
                case EStageSection.Level1:
                    SpawnMonster(UnityEngine.Random.Range(1, 3));
                    break;
                case EStageSection.Level2:
                    SpawnMonster(UnityEngine.Random.Range(2, 4));
                    break;
            }

            float delayTime = UnityEngine.Random.Range(4, 5);
            yield return new WaitForSeconds(delayTime);
        }

        coSpawnMonster = null;
    }

    protected override IEnumerator CoReceiveStageParam()
    {
        while(Managers.Game.IsGamePlay)
        {
            float stageLength = Mathf.Abs(finishLineObject.transform.position.z - playerStartPoint.position.z);
            float goalLength = Mathf.Abs(finishLineObject.transform.position.z - player.transform.position.z);
            stageParam.CurrDisRatio = Mathf.Abs(goalLength / stageLength - 1);

            OnReceiveStageParamCallBack(stageParam);
            yield return new WaitForSeconds(0.5f);
        }

        coReceiveStageParam = null;
    }

    private EStageSection CheckStageSection()
    {
        int goalPercent = (int)(stageParam.CurrDisRatio * 100); // 0 ~ 100

        if (goalPercent < 1)
            return EStageSection.None;
        else if (goalPercent < 50)
            return EStageSection.Level1;
        else
            return EStageSection.Level2;
    }

    private void SpawnItems()
    {
        float lineLength = Mathf.Abs(finishLineObject.transform.position.z) + Mathf.Abs(playerStartPoint.transform.position.z);
        lineLength *= 0.6f;

        float blockLength = lineLength / 6;
        float basicPointZ = playerStartPoint.transform.position.z + blockLength * 1.5f;

        for(int i = 0; i < 4; i++)
        {
            int spawnLine = UnityEngine.Random.Range(0, 4) * 2;
            Vector3 itemSpawnPoint = new Vector3(
                spawnPointList[spawnLine].transform.position.x,
                0,
                basicPointZ + (i * blockLength * 1.5f));

            ObjectCreator.SpawnItem<BoosterItem>(null, itemSpawnPoint);
        }
    }

    private void SpawnMonster(int spawnCount)
    {
        Vector3 spawnPointVec = player.transform.position;
        spawnPointVec.z += 100f;

        switch (spawnCount)
        {
            case 1:
                {
                    int spawnPointNum = UnityEngine.Random.Range(0, 4) * 2;
                    spawnPointVec.x = spawnPointList[spawnPointNum].transform.position.x;
                    ObjectCreator.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointVec);
                }
                break;
            case 2:
                {
                    bool isBigShark = (UnityEngine.Random.value > 0.5f);
                    if(isBigShark)
                    {
                        int spawnPointNum = UnityEngine.Random.Range(0, 3) * 2 + 1;
                        spawnPointVec.x = spawnPointList[spawnPointNum].transform.position.x;
                        ObjectCreator.SpawnMonster<Shark>(EMonsterType.BigShark, spawnPointVec);
                    }
                    else
                    {
                        int spawnPointNum1 = UnityEngine.Random.Range(0, 4) * 2;
                        int spawnPointNum2 = spawnPointNum1 + (UnityEngine.Random.Range(1, 4) * 2);
                        if(spawnPointNum2 >= spawnPointList.Count)
                            spawnPointNum2 -= spawnPointList.Count + 1;

                        spawnPointVec.x = spawnPointList[spawnPointNum1].transform.position.x;
                        ObjectCreator.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointVec);
                        spawnPointVec.x = spawnPointList[spawnPointNum2].transform.position.x;
                        ObjectCreator.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointVec);
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

                        spawnPointVec.x = spawnPointList[spawnPointNum1].transform.position.x;
                        ObjectCreator.SpawnMonster<Shark>(EMonsterType.BigShark, spawnPointVec);
                        spawnPointVec.x = spawnPointList[spawnPointNum1].transform.position.x;
                        ObjectCreator.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointVec);
                    }
                    else
                    {
                        int spawnExceptionNum = UnityEngine.Random.Range(0, 4) * 2;

                        for(int i = 1; i <= 3; i++)
                        {
                            int spawnPointNum = spawnExceptionNum + (i * 2);
                            if (spawnPointNum >= spawnPointList.Count)
                                spawnPointNum -= spawnPointList.Count + 1;

                            spawnPointVec.x = spawnPointList[spawnPointNum].transform.position.x;
                            ObjectCreator.SpawnMonster<Shark>(EMonsterType.Shark, spawnPointVec);
                        }
                    }
                }
                break;
        }

    }
}