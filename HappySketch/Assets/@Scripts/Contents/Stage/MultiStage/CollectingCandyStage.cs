using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CollectingCandyStage : MultiStage
{
    [field: SerializeField, ReadOnly] List<SpawnPointObject> spawnPointList = new List<SpawnPointObject>();

    [field: SerializeField, ReadOnly] CollectingCandyParam stageParam = null;

    [SerializeField, ReadOnly] bool isScoreBuff;

    protected override void Reset()
    {
        base.Reset();

        Transform[] children = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.TryGetComponent<SpawnPointObject>(out SpawnPointObject spawnPoint))
                spawnPointList.Add(spawnPoint);
        }
    }

    private void OnDisable()
    {
        if (coSpawnCandyItem != null)
            StopCoroutine(coSpawnCandyItem);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StageType = EStageType.CollectingCandy;

        return true;
    }

    public override void SetInfo(Player player = null)
    {
        base.SetInfo(player);

        stageParam = new CollectingCandyParam(ETeamType.Left, new int[(int)ECandyItemType.Max], 0);
    }

    public override void StartStage()
    {
        base.StartStage();

        if (coSpawnCandyItem != null)
            StopCoroutine(coSpawnCandyItem);

        coSpawnCandyItem = StartCoroutine(CoSpawnCandyItem());
    }

    public override void ConnectEvents(Action<ETeamType> onEndGameCallBack)
    {
        base.ConnectEvents(onEndGameCallBack);

        player.ConnectCollectingCandyStage(OnCollectCandyItems, OnChangeScoreBuff);

        // 게임 종료 조건 세팅 해야 함 ??
    }

    public void OnCollectCandyItems(List<ECandyItemType> candyItemTypes)
    {
        foreach(ECandyItemType type in candyItemTypes)
        {
            CollectCandyItem(type);
        }

        if (stageParam.CurrScore < 0)
            stageParam.CurrScore = 0;


    }

    private void CollectCandyItem(ECandyItemType candyItemType)
    {
        switch (candyItemType)
        {
            case ECandyItemType.RedCandy:
                stageParam.CurrScore += 100 * (isScoreBuff ? 2 : 1);

                break;
            case ECandyItemType.GreenCandy:
                stageParam.CurrScore += 200 * (isScoreBuff ? 2 : 1);

                break;
            case ECandyItemType.BlueCandy:
                stageParam.CurrScore += 500 * (isScoreBuff ? 2 : 1);

                break;
            case ECandyItemType.BoomCandy:
                stageParam.CurrScore -= 300;

                break;
            case ECandyItemType.StarCandy:

                break;
        }

    }

    public void OnChangeScoreBuff(bool isScoreBuff)
    {
        this.isScoreBuff = isScoreBuff;
    }

    Coroutine coSpawnCandyItem = null;
    private IEnumerator CoSpawnCandyItem()
    {
        while(Managers.Game.IsGamePlay)
        {
            yield return new WaitForSeconds(5f); // 임시

        }

        coSpawnCandyItem = null;
    }

    private void SpawnCandyItem()
    {

    }
}
