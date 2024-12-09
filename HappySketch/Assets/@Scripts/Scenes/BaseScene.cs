using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public abstract class BaseScene : InitBase
{
    public EScene SceneType { get; protected set; } = EScene.Unknown;

    private void Start()
    {
        switch (SceneType)
        {
            case EScene.TitleScene:
                Managers.Sound.PlayBgm(EBgmSoundType.Title);
                break;
            case EScene.GameScene:
                Managers.Sound.PlayBgm(EBgmSoundType.GameDescription);
                break;
            default:
                Managers.Sound.StopBgm();
                break;
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Init();
        Managers.Scene.SetCurrentScene(this);
        LightingController.InitLighting();

        return true;
    }

    public abstract void Clear();
}