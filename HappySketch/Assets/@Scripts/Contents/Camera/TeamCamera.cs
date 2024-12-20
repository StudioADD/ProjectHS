using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class CameraInfoData
{
    public float fieldOfView = 60f;
    public float targetDistance = 5f;
    public float cameraHeight = 0f;
    public float lookAtHeight = 0f;
    public float nearClipping = 0.3f;
    public float farClipping = 1000f;

    public CameraInfoData(float fieldOfView, float targetDistance, float cameraHeight, float lookAtHeight, float nearClipping, float farClipping)
    {
        this.fieldOfView = fieldOfView;
        this.targetDistance = targetDistance;
        this.cameraHeight = cameraHeight;
        this.lookAtHeight = lookAtHeight;
        this.nearClipping = nearClipping;
        this.farClipping = farClipping;
    }
}

public class TeamCamera : InitBase
{
    [SerializeField, ReadOnly] protected Camera cam;
    [SerializeField, ReadOnly] protected BaseObject target = null;

    [SerializeField, ReadOnly]
    protected CameraInfoData cameraInfoData = null;

    private void LateUpdate()
    {
        if (cameraInfoData != null && target != null)
        {
            FollowingTarget();
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        cam = GetComponent<Camera>();
        
        return true;
    }

    public void SetInfo(EStageType stageType)
    {
        LoadCameraDataInfo(stageType);
        cam.fieldOfView = cameraInfoData.fieldOfView;
        cam.nearClipPlane = cameraInfoData.nearClipping;
        cam.farClipPlane = cameraInfoData.farClipping;
    }

    public virtual void SetTarget(BaseObject target)
    {
        this.target = target;
    }

    public Camera GetTeamCamera() => cam;

    private void FollowingTarget()
    {
        Vector3 targetPos = target.transform.position;
        transform.position = targetPos + new Vector3(0, cameraInfoData.cameraHeight, cameraInfoData.targetDistance * -1);
        transform.LookAt(targetPos + new Vector3(0, cameraInfoData.lookAtHeight, 0));
    }

    protected bool LoadCameraDataInfo(EStageType stageType)
    {
#if UNITY_EDITOR
        string loadPath = Application.dataPath + DataPath.STAGE_JSONDATA_PATH + $"/CameraData{(int)stageType}.json";
        
        if (!File.Exists(loadPath))
        {
            Debug.LogWarning($"로드할 데이터가 없습니다.\n경로 : {loadPath}");
            return false;
        }

        string jsonData = File.ReadAllText(loadPath);
        cameraInfoData = JsonUtility.FromJson<CameraInfoData>(jsonData);

        return true;
#else
        TextAsset jsonData = Managers.Resource.Load<TextAsset>($"Data/StageData/CameraData{(int)stageType}");
        cameraInfoData = JsonUtility.FromJson<CameraInfoData>(jsonData.text);
        return true;
#endif

    }
}