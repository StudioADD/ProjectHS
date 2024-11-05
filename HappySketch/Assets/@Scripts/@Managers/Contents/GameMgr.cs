using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr
{
    private int currStageId = 1;

    public void Init()
    {
        currStageId = 1;
    }

    public void Clear()
    {

    }

    public void StartGame()
    {
        StartStage(currStageId);
    }

    public void EndGame()
    {

    }

    private void StartStage(int stageId)
    {
        if(Managers.Scene.CurrScene is GameSceneView gameScene)
        {
            gameScene.StartGame((EStageType)currStageId);
        }
    }
    
    public void EndStage()
    {

    }
}
