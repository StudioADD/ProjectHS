using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

/// <summary>
/// 에디터에서만 사용하게 될 클래스
/// </summary>
public class CameraEditor : TeamCamera
{
    [Header ("[ 카메라 세팅 영역 ]")]
    [SerializeField] EStageType stageType = EStageType.None;
    [SerializeField] BaseObject testTarget;

    [Range(0f, 180f)]
    [SerializeField] float fieldOfView = 60f;

    [Range(5f, 15f)]
    [SerializeField] float targetDistance = 5f;

    [Range(-10f, 10f)]
    [SerializeField] float cameraHeight = 5f;

    [Range(-10f, 10f)]
    [SerializeField] float lookAtHeight = 0f;

    private CameraInfoData cameraInfo;


    private void Update()
    {
        if(testTarget != null)
            SetTarget(testTarget);

        cameraInfo = new CameraInfoData(fieldOfView, targetDistance, cameraHeight, lookAtHeight);
        SetInfo(cameraInfo);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void InitCameraInfo()
    {
        fieldOfView = 60f;
        targetDistance = 5f;
        cameraHeight = 5f;
        lookAtHeight = 0f;
    }

    private void SetCameraInfo(CameraInfoData cameraInfoData)
    {
        fieldOfView = cameraInfoData.fieldOfView;
        targetDistance = cameraInfoData.targetDistance;
        cameraHeight = cameraInfoData.cameraHeight;
        lookAtHeight = cameraInfoData.lookAtHeight;
    }

    public void LoadCameraInfo()
    {
        if(stageType == EStageType.None)
        {
            Debug.LogWarning("스테이지 타입을 설정하지 않았습니다.");
            return;
        }

        string loadPath = Application.dataPath + AssetsPath.STAGE_JSONDATA_PATH + $"/CameraData{(int)stageType}.json";
        
        if(!File.Exists(loadPath))
        {
            Debug.LogWarning($"로드할 데이터가 없습니다.\n경로 : {loadPath}");
            return;
        }

        string jsonData = File.ReadAllText(loadPath);
        CameraInfoData cameraInfoData = JsonUtility.FromJson<CameraInfoData>(jsonData);
        SetCameraInfo(cameraInfoData);
    }
    
    public void SaveCameraInfo()
    {
        if (stageType == EStageType.None)
        {
            Debug.LogWarning("스테이지 타입을 설정하지 않았습니다.");
            return;
        }

        string savePath = Application.dataPath + AssetsPath.STAGE_JSONDATA_PATH + $"/CameraData{(int)stageType}";
        Util.Editor_FileDelete(savePath);
        
        string jsonData = JsonUtility.ToJson(cameraInfo);
        File.WriteAllText(savePath + ".json", jsonData);
    }
}
