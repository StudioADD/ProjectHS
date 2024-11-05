using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageEditor))]
public class EditStage : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StageEditor stageEditor = (StageEditor)target;

        GUILayout.Space(10);
        GUILayout.Label("[ 스테이지 에디터 ]", EditorStyles.boldLabel);


    }
}
