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

        stageParam = new CollectingCandyParam(ETeamType.Left, new int[(int)ECandyItemType.Max], 0);

        scoreCollector = FindObjectOfType<ScoreCollector>();
        if(scoreCollector == null)
            scoreCollector = new GameObject("@ScoreCollector").AddComponent<ScoreCollector>();
    }

    public override void StartStage()
    {
        base.StartStage();

        scoreCollector.OnGameTimerEnd -= GameTimerEndCallBack;
        scoreCollector.OnGameTimerEnd += GameTimerEndCallBack;

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

    public void SpawnCandyItems()
    {
        float distance = MathF.Abs(playerStartPoint.position.z - playerEndPoint.position.z);
        Vector3 currPoint = playerStartPoint.position;

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
}
