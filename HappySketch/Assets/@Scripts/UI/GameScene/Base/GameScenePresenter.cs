using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MomDra
{
    public class GameScenePresenter
    {
        protected GameSceneView view;
        protected GameSceneModel model;

        public GameScenePresenter(GameSceneView view, GameSceneModel model)
        {
            this.view = view;
            this.model = model;
            view.SetGameScenePresenter(this);
        }
    }
}
