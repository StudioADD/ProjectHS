using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageEditor))]
public class EditStage : Editor
{
    bool isSpawnStageUnlocked = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StageEditor stageEditor = (StageEditor)target;

        GUILayout.Space(10);
        GUILayout.Label("[ 스테이지 에디터 ]", EditorStyles.boldLabel);

        GUILayout.Space(10);
        isSpawnStageUnlocked = EditorGUILayout.Toggle("스테이지 소환 잠금 해제", isSpawnStageUnlocked);
        GUILayout.Space(5);
        if (GUILayout.Button("스테이지 소환") && isSpawnStageUnlocked)
        {
            isSpawnStageUnlocked = false;
            stageEditor.SpawnStage();
            Debug.Log("스테이지 소환 완료");
        }

        GUILayout.Space(20);
    }
}
