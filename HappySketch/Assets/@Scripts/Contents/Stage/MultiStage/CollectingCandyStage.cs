using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CollectingCandyStage : MultiStage
{
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
        player.ConnectCollectingCandyStage(OnCollectCandyItem, OnChangeScoreBuff);


    }

    public void OnCollectCandyItem(ECandyItemType candyItemType)
    {
           
    }

    public void OnChangeScoreBuff(bool isScoreBuff)
    {

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
}
