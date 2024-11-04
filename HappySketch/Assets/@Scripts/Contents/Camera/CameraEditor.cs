#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;

public class CameraEditor : TeamCamera
{
    [Header ("[ 카메라 세팅 영역 ]")]
    [Space(5f)] [SerializeField]
    EStageType stageType = EStageType.None;
    
    [Space(5f)] [SerializeField]
    BaseObject testTarget;

    [Space(10f)] [Range(0f, 180f)] 
    [SerializeField] float fieldOfView = 60f;

    [Range(5f, 15f)] 
    [SerializeField] float targetDistance = 5f;
    
    [Range(-10f, 10f)] 
    [SerializeField] float cameraHeight = 5f;
    
    [Range(-10f, 10f)] 
    [SerializeField] float lookAtHeight = 0f;

    private void Update()
    {
        if(testTarget != null)
            SetTarget(testTarget);

        UpdateCameraInfo();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        stageType = EStageType.None;
        cameraInfoData = new CameraInfoData(fieldOfView, targetDistance, cameraHeight, lookAtHeight);

        return true;
    }

    public void InitCameraInfo()
    {
        stageType = EStageType.None;
        fieldOfView = 60f;
        targetDistance = 5f;
        cameraHeight = 5f;
        lookAtHeight = 0f;
    }

    private void UpdateCameraInfo()
    {
        cameraInfoData.fieldOfView = fieldOfView;
        cameraInfoData.targetDistance = targetDistance;
        cameraInfoData.cameraHeight = cameraHeight;
        cameraInfoData.lookAtHeight = lookAtHeight;
        base.SetInfo(stageType);
    }

    public bool LoadCameraInfo()
    {
        if(stageType == EStageType.None)
        {
            Debug.LogWarning("스테이지 타입을 설정하지 않았습니다.");
            return false;
        }

        bool isLoad = base.LoadCameraDataInfo(stageType);

        if(isLoad)
        {
            fieldOfView = cameraInfoData.fieldOfView;
            targetDistance = cameraInfoData.targetDistance;
            cameraHeight = cameraInfoData.cameraHeight;
            lookAtHeight = cameraInfoData.lookAtHeight;
        }

        return isLoad;
    }
    
    public bool SaveCameraInfo()
    {
        if (stageType == EStageType.None)
        {
            Debug.LogWarning("스테이지 타입을 설정하지 않았습니다.");
            return false;
        }

        string savePath = Application.dataPath + DataPath.STAGE_JSONDATA_PATH + $"/CameraData{(int)stageType}";
        Util.Editor_FileDelete(savePath);
        
        string jsonData = JsonUtility.ToJson(cameraInfoData);
        File.WriteAllText(savePath + ".json", jsonData);

        return true;
    }
}
#endif