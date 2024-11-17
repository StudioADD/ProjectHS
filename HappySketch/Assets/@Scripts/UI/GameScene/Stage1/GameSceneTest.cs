using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSceneTest : MonoBehaviour
{
    [SerializeField]
    private UI_GameScene gameScene;

    private float ratio = 0f;

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    gameScene.ReceiveData(new UIBoosterCountData(EStageType.SharkAvoidance, Define.ETeamType.Left, 2));
        //}

        //if(Input.GetKeyDown(KeyCode.S))
        //{
        //    ratio += 0.0055f;
        //    gameScene.ReceiveData(new UIRatioData(EStageType.SharkAvoidance, Define.ETeamType.Left, ratio));
        //}

        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameScene.StartStage(EStageType.CollectingCandy);
        }
    }
}
