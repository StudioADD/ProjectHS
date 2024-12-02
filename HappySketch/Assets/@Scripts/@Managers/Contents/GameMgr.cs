using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameMgr
{
    public bool IsGamePlay { get; private set; } = false;

    int[] winnerCounts = new int[2]; // Left, Right
    bool[] playedStages = new bool[(int)EStageType.Max];

    int currStageId = 0;

    public void Init()
    {

    }

    public void Clear()
    {
        currStageId = 0;

        for (int i = 0; i < winnerCounts.Length; i++)
            winnerCounts[i] = 0;

        for (int i = 0; i < playedStages.Length; i++)
            playedStages[i] = false;
    }

    public int GetWinnerTeamCount(ETeamType teamType)
    {
        return winnerCounts[(int)teamType];
    }

    public void SetStageId(int stageId = -1)
    {
        if(stageId == -1)
        {
            currStageId++;
            return;
        }

        currStageId = stageId;
    }

    private void EndGame(ETeamType winnerTeam)
    {
        Managers.Scene.LoadScene(EScene.ResultScene);
        Clear();
    }

    public EStageType GetCurrStageType()
    {
        if(currStageId == 0)
        {
            Debug.LogWarning("currStage가 세팅되지 않음");
            return 0;
        }

        return (EStageType)currStageId;
    }
    
    public void SetStageInfo()
    {
        if (Managers.UI.SceneUI is UI_GameScene uiGameScene)
        {
            uiGameScene.SetInfo((EStageType)currStageId);
        }

        if (Managers.Scene.CurrScene is GameScene gameScene)
        {
            playedStages[currStageId] = true;
            gameScene.SetStageInfo((EStageType)currStageId);
        }
    }

    public void StartStage()
    {
        IsGamePlay = true;
    }

    public void EndStage(ETeamType winnerTeam)
    {
        IsGamePlay = false;
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
