using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    [SerializeField, ReadOnly] BaseStageController stageController;
    [SerializeField] CameraGroupController cameraGroupController;

    protected virtual void Reset()
    {
        cameraGroupController = Util.FindChild<CameraGroupController>(gameObject);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.GameScene;
        Managers.Game.SetStageId();

        return true;
    }

    public void SetStageInfo(EStageType stageType)
    {
        Managers.Sound.PlayBgm(Util.ParseEnum<EBgmSoundType>(Util.EnumToString(stageType)));
        Managers.Sound.ChangeBGMSpeed(1.2f, 5f);

        // StageController
        Type type = Type.GetType($"{stageType}Stage");
        GameObject stageControllerObj = new GameObject("StageController");
        stageControllerObj.transform.SetParent(transform, false);

        if (type.BaseType == typeof(SingleStage))
        {
            stageController = stageControllerObj.AddComponent<SingleStageController>();
            
        }
        else if(type.BaseType == typeof(MultiStage))
        {
            stageController = stageControllerObj.AddComponent<MultiStageController>();
        }
        else
        {
            Debug.LogWarning($"없는 타입 : {type.Name}");
            return;
        }

        // Player
        Player leftPlayer = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_PLAYER_PATH}/LeftPlayer").GetComponent<Player>();
        Player rightPlayer = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_PLAYER_PATH}/RightPlayer").GetComponent<Player>();
        stageController.SetInfo(stageType, leftPlayer, rightPlayer);

        // Camera
        cameraGroupController.SetInfo(stageType);
        cameraGroupController.SetTarget(leftPlayer, ETeamType.Left); 
        cameraGroupController.SetTarget(rightPlayer, ETeamType.Right);

        // Connect Events
        stageController.ConnectEvents();
        
        // UI ( GameStart )
        if (Managers.UI.SceneUI is UI_GameScene uI_GameScene)
        {
            uI_GameScene.StartStage();
            
            switch(stageController)
            {
                case MultiStageController multiStageController:
                    uI_GameScene.ConnectStageEvents(multiStageController.GetStage(ETeamType.Left));
                    uI_GameScene.ConnectStageEvents(multiStageController.GetStage(ETeamType.Right));
                    break;
                case SingleStageController singleStageController:
                    uI_GameScene.ConnectStageEvents(singleStageController.GetStage());
                    break;
            }
        }

        // UI Start Effect ( 3, 2, 1 )
        UIGameStartCounterParam param = new UIGameStartCounterParam(3, StartStage);
        coWaitCondition = StartCoroutine(CoWaitCondition(
           () => Managers.UI.IsPopupActiveSelf<UI_FadeEffectPopup>() == false,
           () => Managers.UI.SpawnObjectUI<UI_GameStartCounter>(param)
           ));
    }

    Coroutine coWaitCondition = null;
    private IEnumerator CoWaitCondition(Func<bool> conditionFunc, Action onCondition)
    {
        while (!conditionFunc.Invoke())
            yield return null;

        onCondition?.Invoke();
    }

    public void  StartStage()
    {
        Managers.Game.StartStage();
        stageController.StartStage();
    }

    public override void Clear()
    {
        
    }
}
