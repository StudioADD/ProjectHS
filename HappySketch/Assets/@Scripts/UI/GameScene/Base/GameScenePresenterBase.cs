using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MomDra
{
    public class GameScenePresenterBase
    {
        protected GameSceneViewBase view;
        protected GameSceneModelBase model;

        public GameScenePresenterBase(GameSceneViewBase view, GameSceneModelBase model)
        {
            this.view = view;
            this.model = model;
            view.SetGameScenePresenter(this);
        }
    }
}
