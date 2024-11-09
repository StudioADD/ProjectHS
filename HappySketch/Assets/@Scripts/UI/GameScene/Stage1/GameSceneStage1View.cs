using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
 
namespace MomDra
{
    public class GameSceneStage1View : ViewBase
    {
        [SerializeField]
        private Image progressing;

        [SerializeField]
        private Image[] items;

        public void UpdateItemCount(int count)
        {
            Debug.Log($"아이템 개수: {count}");

            for(int i = 0; i < count; ++i)
            {
                items[i].gameObject.SetActive(true);
            }

            for(int i = count; i < items.Length; ++i)
            {
                items[i].gameObject.SetActive(false);
            }
        }

        public void UpdateLeftProgressRatio(ETeamType teamType, float ratio)
        {

        }

        public void UpdateRightProgressRatio(ETeamType teamType, float ratio)
        {

        }
    }
}
