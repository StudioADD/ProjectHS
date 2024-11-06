using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MomDra
{
    public class GameSceneView : MonoBehaviour
    {
        protected GameScenePresenter gameScenePresenter;

        public void SetGameScenePresenter(GameScenePresenter gameScenePresenter)
        {
            this.gameScenePresenter = gameScenePresenter;
        }
    }
}
