using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraEditor))]
public class EditCamera : Editor
{
    bool isInitUnlocked = false;
    bool isLoadUnlocked = false;
    bool isSaveUnocked = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CameraEditor cameraEditor = (CameraEditor)target;

        GUILayout.Space(10);
        GUILayout.Label("[ 카메라 에디터 ]", EditorStyles.boldLabel);

        GUILayout.Space(10);
        isInitUnlocked = EditorGUILayout.Toggle("초기 세팅 잠금 해제 ", isInitUnlocked);
        GUILayout.Space(5);
        if (GUILayout.Button("초기 값으로 세팅") && isInitUnlocked)
        {
            isInitUnlocked = false;
            cameraEditor.InitCameraInfo();
            Debug.Log("초기 값으로 세팅 완료");
        }

        // 데이터 불러오기
        GUILayout.Space(5);
        isLoadUnlocked = EditorGUILayout.Toggle("불러오기 잠금 해제 ", isLoadUnlocked);
        GUILayout.Space(5);
        if (GUILayout.Button("데이터 불러오기") && isLoadUnlocked)
        {
            bool isLoad = cameraEditor.LoadCameraInfo();
            isLoadUnlocked = false;
            
            if(isLoad)
                Debug.Log("스테이지 데이터 불러오기 완료");
        }

        // 데이터 저장
        GUILayout.Space(5);
        isSaveUnocked = EditorGUILayout.Toggle("저장하기 잠금 해제 ", isSaveUnocked);
        GUILayout.Space(5);
        if (GUILayout.Button("데이터 저장하기") && isSaveUnocked)
        {
            bool isSave = cameraEditor.SaveCameraInfo();
            isSaveUnocked = false;
            
            if(isSave)
                Debug.Log("스테이지 데이터 저장하기 완료");
        }

        GUILayout.Space(20);
    }
}