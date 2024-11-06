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
    [SerializeField, ReadOnly] BaseStage currStage = null;

    [Header("[ 카메라 세팅 영역 ]")]
    [Space(5f)] [SerializeField]
    EStageType stageType = EStageType.None;
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        stageType = EStageType.None;

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

        if (stageType == EStageType.None)
        {
            Debug.LogWarning("소환할 스테이지 타입을 설정해주세요.");
            return;
        }

        string loadPath = $"{Application.dataPath}/Resources/Prefabs/{PrefabPath.STAGE_PATH}/{stageType}.prefab";
        GameObject go = PrefabUtility.LoadPrefabContents(loadPath);
        Util.Editor_InstantiateObject(go);

        // currStage = (BaseStage)PrefabUtility.InstantiatePrefab(currStage).GetComponent<BaseStage>();
        // currStage.gameObject.transform.position = Vector3.zero;
    }
}
#endif