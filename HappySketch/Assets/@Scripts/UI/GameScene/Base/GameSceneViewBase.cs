using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MomDra
{
    public abstract class GameSceneViewBase : MonoBehaviour
    {
        protected GameScenePresenterBase gameScenePresenter;

        public void SetGameScenePresenter(GameScenePresenterBase gameScenePresenter)
        {
            this.gameScenePresenter = gameScenePresenter;
        }
    }
}
