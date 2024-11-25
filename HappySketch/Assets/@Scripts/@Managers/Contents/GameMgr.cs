using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameMgr
{
    int[] winnerCounts = new int[2];
    bool[] playedStages = new bool[(int)EStageType.Max - 1];

    int currStageId = 0;

    public void Init()
    {

    }

    public void Clear()
    {
        for (int i = 0; i < winnerCounts.Length; i++)
            winnerCounts[i] = 0;

        for (int i = 0; i < playedStages.Length; i++)
            playedStages[i] = false;
    }

    private void EndGame(ETeamType winnerTeam)
    {
        // 타이틀씬으로 (임시)
        Managers.Scene.LoadScene(EScene.ResultScene);
        Clear();
    }

    public EStageType GetCurrStageType()
    {
        return (EStageType)currStageId;
    }
    
    public void SetStageInfo()
    {
        currStageId++;

        if (Managers.Scene.CurrScene is GameScene gameScene)
        {
            playedStages[currStageId - 1] = true;
            gameScene.SetStageInfo((EStageType)currStageId);
        }
    }

    public void EndStage(ETeamType winnerTeam)
    {
        winnerCounts[(int)winnerTeam] += 1;

        if (winnerCounts[(int)winnerTeam] >= ((int)EStageType.Max / 2))
        {
            EndGame(winnerTeam);
        }
        else
        {
            Managers.Scene.LoadScene(EScene.GameScene);
        }
    }
}
