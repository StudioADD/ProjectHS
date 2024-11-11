using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr
{
    bool[] playedStages = new bool[(int)EStageType.Max - 1];

    public void Init()
    {

    }

    public void Clear()
    {

    }
      

    public void StartGame()
    {
        for(int i = 0; i < playedStages.Length; i++)
            playedStages[i] = false;

        StartStage();
    }

    public void EndGame()
    {

    }

    private void StartStage()
    {
        int stageId = 0;
        while (playedStages[stageId])
            stageId = Random.Range(1, (int)EStageType.Max - 1);

        stageId = 1; // 테스트

        if(Managers.Scene.CurrScene is GameScene gameScene)
        {
            playedStages[stageId - 1] = true;
            gameScene.StartStage((EStageType)stageId);
        }
    }
    
    public void EndStage()
    {

    }
}
