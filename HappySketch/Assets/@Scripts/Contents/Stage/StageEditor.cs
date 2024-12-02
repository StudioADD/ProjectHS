#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Define;

public enum EMaterialType
{
    Red = 0,
    Blue,

    Max
}

public class StageEditor : InitBase
{
    [SerializeField, ReadOnly] CameraEditor cameraEditor;
    [SerializeField, ReadOnly] BaseStage currStage = null;

    [SerializeField, ReadOnly] Player player = null;
    [SerializeField, ReadOnly] Transform playerStartPoint;

    [Header("[ 스테이지 세팅 영역 ]")]
    [Space(5f)]
    public EStageType StageType = EStageType.None;

    private void Start()
    {
        PlayStage();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        cameraEditor = FindObjectOfType<CameraEditor>();
        currStage = FindObjectOfType<BaseStage>();
        StageType = currStage.StageType;

        if (currStage == null)
        {
            Debug.LogError("스테이지를 소환하고 테스트해주세요.");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        return true;
    }
    // 현재 소환된 스테이지를 하나로 보장해줘야 됨.

    public void SpawnStage()
    {
        currStage = FindObjectOfType<BaseStage>();
        if (currStage != null)
        {
            GameObject.DestroyImmediate(currStage.gameObject);
        }

        if (StageType == EStageType.None)
        {
            Debug.LogWarning("소환할 스테이지 타입을 설정해주세요.");
            return;
        }

        string loadPath = $"{Application.dataPath}/Resources/Prefabs/{PrefabPath.STAGE_PATH}/{StageType}.prefab";
        GameObject go = PrefabUtility.LoadPrefabContents(loadPath);
        Util.Editor_InstantiateObject(go);
    }

    public void PlayStage()
    {
        Managers.Game.SetStageId((int)StageType);
        Managers.Game.StartStage();

        LightingController.SetStageLighting(StageType);
        playerStartPoint = Util.FindChild<Transform>(currStage.gameObject, "PlayerStartPoint", true);

        player = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_PLAYER_PATH}/LeftPlayer").GetComponent<Player>();
        player.transform.position = playerStartPoint.position;
        player.transform.position += Vector3.up * player.GetColliderHeight();
        player.SetInfo((int)StageType);

        cameraEditor.SetTarget(player);
        
        switch(currStage)
        {
            case MultiStage multiStage:
                multiStage.SetInfo(player);
                multiStage.ConnectEvents(null);
                break;
            case SingleStage singleStage:
                singleStage.SetInfo(player, player);
                singleStage.ConnectEvents(null);
                break;
        }

        currStage.StartStage();
    }
}
#endif