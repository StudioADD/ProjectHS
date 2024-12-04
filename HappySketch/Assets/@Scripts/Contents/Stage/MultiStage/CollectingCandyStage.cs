using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using CollectingCandy;

public class CollectingCandyStage : MultiStage
{
    [SerializeField, ReadOnly] ScoreCollector scoreCollector;
    [SerializeField, ReadOnly] Transform playerEndPoint;

    [field: SerializeField, ReadOnly]
    readonly ECandyItemType[] CANDYITEMS =
        {
        ECandyItemType.RedCandyItem, ECandyItemType.RedCandyItem,
        ECandyItemType.GreenCandyItem, ECandyItemType.GreenCandyItem,
        ECandyItemType.BlueCandyItem, ECandyItemType.BoomCandyItem
        };
    
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

        playerEndPoint = Util.FindChild<Transform>(gameObject, "PlayerEndPoint", true);
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

        stageParam = new CollectingCandyParam(TeamType, new int[(int)ECandyItemType.Max], 0);
        SpawnCandyItemWaves();

        scoreCollector = FindObjectOfType<ScoreCollector>();
        if(scoreCollector == null)
            scoreCollector = new GameObject("@ScoreCollector").AddComponent<ScoreCollector>();
        scoreCollector.OnGameTimerEnd += GameTimerEndCallBack;
    }

    public override void StartStage()
    {
        base.StartStage();
        
        // 임시
        if (TeamType == ETeamType.Left)
            scoreCollector.StartStage();
    }

    public override void EndStage(ETeamType winnerTeam)
    {
        base.EndStage(winnerTeam);

        // 일단 필요없을 거 같고
    }

    public void GameTimerEndCallBack()
    { 
        scoreCollector.SetTotalScore(stageParam);
    }

    public override void ConnectEvents(Action<ETeamType> onEndGameCallBack)
    {
        base.ConnectEvents(onEndGameCallBack);

        player.ConnectCollectingCandyStage(OnCollectCandyItems, OnChangeScoreBuff);

        if (scoreCollector != null)
        {
            scoreCollector.OnEndGameCallBack -= onEndGameCallBack;
            scoreCollector.OnEndGameCallBack += onEndGameCallBack;
        }
#if DEBUG
        else
            Debug.LogWarning("scoreCollector is Null!!");
#endif
    }

    private void SpawnCandyItemWaves()
    {
        float distance = MathF.Abs(playerStartPoint.position.z - playerEndPoint.position.z);
        float currPos = playerStartPoint.position.z + (distance / 20);
        distance *= 0.9f;

        int starItemSpawnPoint = UnityEngine.Random.Range(9, 16); // 10 ~ 15
        bool isStarItem = false;

        for(int i = 0; i < 20; i++)
        {
            isStarItem = starItemSpawnPoint == i;

            currPos += distance / 20;
            SpawnCandyItemWave(currPos, isStarItem);
        }
    }

    private void SpawnCandyItemWave(float spawnPosZ, bool isStarItem = false)
    {
        CANDYITEMS.Shuffle();

        for (int i = 0; i < spawnPointList.Count; i++)
        {
            Vector3 spawnPoint = new Vector3(
                spawnPointList[i].transform.position.x,
                spawnPointList[i].transform.position.y,
                spawnPosZ);

            CandyItemParam param = new CandyItemParam(CANDYITEMS[i]);

            if (isStarItem == true && param.CandyItemType == ECandyItemType.BoomCandyItem)
                param.CandyItemType = ECandyItemType.StarCandyItem;

            ObjectCreator.SpawnItem<CandyItem>(param, spawnPoint);
        }
    }

    public void OnCollectCandyItems(List<ECandyItemType> candyItemTypes)
    {
        foreach(ECandyItemType type in candyItemTypes)
        {
            CollectCandyItem(type);
        }

        if (stageParam.CurrScore < 0)
            stageParam.CurrScore = 0;

        // UI에 이벤트 넘겨주기
    }

    private void CollectCandyItem(ECandyItemType candyItemType)
    {
        switch (candyItemType)
        {
            case ECandyItemType.RedCandyItem:
                stageParam.CurrScore += 100 * (isScoreBuff ? 2 : 1);

                break;
            case ECandyItemType.GreenCandyItem:
                stageParam.CurrScore += 200 * (isScoreBuff ? 2 : 1);

                break;
            case ECandyItemType.BlueCandyItem:
                stageParam.CurrScore += 500 * (isScoreBuff ? 2 : 1);

                break;
            case ECandyItemType.BoomCandyItem:
                stageParam.CurrScore -= 300;

                break;
            case ECandyItemType.StarCandyItem:

                break;
        }

    }

    public void OnChangeScoreBuff(bool isScoreBuff)
    {
        this.isScoreBuff = isScoreBuff;
    }
}
