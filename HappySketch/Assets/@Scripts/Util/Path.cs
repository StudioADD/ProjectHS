using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "Assets/" 이후 경로
/// </summary>
public static class DataPath
{
    // Json Data
    public const string STAGE_JSONDATA_PATH = "/Resources/Data/StageData";
}

/// <summary>
/// "Assets/Resource/" 이후 경로
/// </summary>
public static class LoadPath
{

}

/// <summary>
/// "Assets/Resource/Prefabs/" 이후 경로
/// </summary>
public static class PrefabPath
{
    public const string INPUTMANAGER_PATH = "InputManager/InputManager";

    // Stage
    public const string STAGE_PATH = "Stage";

    // Object
    public const string OBJECT_PATH = "Object";
    public const string OBJECT_PLAYER_PATH = "Object/Player";
    public const string OBJECT_MONSTER_PATH = "Object/Monster";

    // UI
    public const string UI_PATH = "UI";
    public const string UI_POPUP_PATH = "UI/Popup";
    public const string UI_OBJECT_PATH = "UI/Object";
}
