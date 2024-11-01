using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    [SerializeField] CameraGroupController cameraGroup;

    private void Start()
    {
        SetInfo();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.GameScene;

        return true;
    }

    private void SetInfo()
    {
        cameraGroup.SetInfo();
    }

    public override void Clear()
    {

    }
}
