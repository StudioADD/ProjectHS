using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MomDra
{
    public class GameSceneView : MonoBehaviour
    {
        [SerializeField]
        private Image progressing;

        [SerializeField]
        private Image[] items;

        private GameScenePresenter gameScenePresenter;

        public void SetGameScenePresenter(GameScenePresenter gameScenePresenter)
        {
            this.gameScenePresenter = gameScenePresenter;
        }

        public void UpdateItemCount(int count)
        {
            for(int i = 0; i < count; ++i)
            {
                items[i].gameObject.SetActive(true);
            }

            for(int i = count; i < items.Length; ++i)
            {
                items[i].gameObject.SetActive(false);
            }
        }

        public void UpdateProgressRatio(float ratio)
        {
            progressing.fillAmount = ratio;
        }
    }
}
