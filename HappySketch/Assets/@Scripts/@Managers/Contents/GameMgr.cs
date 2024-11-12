using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    }


    public void StartGame()
    {
        for (int i = 0; i < winnerCounts.Length; i++)
            winnerCounts[i] = 0;

        for (int i = 0; i < playedStages.Length; i++)
            playedStages[i] = false;

        StartStage();
    }

    private void EndGame(ETeamType winnerTeam)
    {
        // Result
    }

    private void StartStage()
    {
        currStageId++;

        if (Managers.Scene.CurrScene is GameScene gameScene)
        {
            playedStages[currStageId - 1] = true;
            gameScene.StartStage((EStageType)currStageId);
        }
    }

    public void EndStage(ETeamType winnerTeam)
    {
        winnerCounts[(int)winnerTeam] += 1;

        if (winnerCounts[(int)winnerTeam] >= (int)EStageType.Max / 2)
        {
            EndGame(winnerTeam);
        }
        else
        {
            Managers.Scene.LoadScene(EScene.GameScene);
        }
    }
}
