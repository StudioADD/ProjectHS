using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MomDra
{
    public class TitleScenePlayerView : ViewBase
    {
        private Image playerImage;

        private void Awake()
        {
            playerImage = GetComponent<Image>();
        }

        public void PlayButtonClicked()
        {
            Debug.Log("PlayButtonClicked");
        }
    }
}
