using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MomDra
{
    public class TitleSceneDefaultView : ViewBase
    {
        [SerializeField]
        private Image[] images;

        private void Awake()
        {
            gameObject.GetComponentInChildren<Button>().onClick.AddListener(PlayButtonClicked);
        }

        public void PlayButtonClicked()
        {

        }
    }
}
