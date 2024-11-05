using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MomDra;

public class UI_GameScene : UI_BaseScene
{
    private GameScenePresenter presenter1P;
    private GameScenePresenter presenter2P;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        MomDra.GameSceneView[] gameSceneViews = GetComponents<MomDra.GameSceneView>();

        presenter1P = new GameScenePresenter(gameSceneViews[0]);
        presenter2P = new GameScenePresenter(gameSceneViews[1]);

        return true;
    }
}
