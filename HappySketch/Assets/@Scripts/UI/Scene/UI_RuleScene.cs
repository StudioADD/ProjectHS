using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RuleScene : UI_BaseScene
{
    [SerializeField]
    private float automaticNextSceneTime = 5.0f;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    private void Start()
    {
        ChangeSceneAfterTime(Define.EScene.GameScene, automaticNextSceneTime);
    }
}
