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

    public CameraInfoData(float fieldOfView, float targetDistance, float cameraHeight, float lookAtHeight)
    {
        this.fieldOfView = fieldOfView;
        this.targetDistance = targetDistance;
        this.cameraHeight = cameraHeight;
        this.lookAtHeight = lookAtHeight;
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

    public void SetInfo(EStageType stageType, int stageDisX)
    {
        LoadCameraDataInfo(stageType);
        cam.fieldOfView = cameraInfoData.fieldOfView;
        this.transform.position = new Vector3(stageDisX, stageDisX, 0f);
    }

    public void SetTarget(BaseObject target) => this.target = target;

    private void FollowingTarget()
    {
        transform.position = target.transform.position + new Vector3(0, cameraInfoData.cameraHeight, cameraInfoData.targetDistance * -1);
        transform.LookAt(target.transform.position + new Vector3(0, cameraInfoData.lookAtHeight, 0));
    }

    protected bool LoadCameraDataInfo(EStageType stageType)
    {
        string loadPath = Application.dataPath + DataPath.STAGE_JSONDATA_PATH + $"/CameraData{(int)stageType}.json";

        if (!File.Exists(loadPath))
        {
            Debug.LogWarning($"로드할 데이터가 없습니다.\n경로 : {loadPath}");
            return false;
        }

        string jsonData = File.ReadAllText(loadPath);
        cameraInfoData = JsonUtility.FromJson<CameraInfoData>(jsonData);
        
        return true;
    }
}


public class A : MonoBehaviour
{
    [SerializeField] B b;

    public void Test()
    {
        b.OnTriggerEvent -= OnTriggerEvent;
        b.OnTriggerEvent += OnTriggerEvent;
    }

    public void OnTriggerEvent(Collider other)
    {

    }
}

public class B : MonoBehaviour
{
    public event Action<Collider> OnTriggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEvent?.Invoke(other);
        // C# ? -> 이새기 널이야? -> 널 아니면 뒤에 실행.
    }
}