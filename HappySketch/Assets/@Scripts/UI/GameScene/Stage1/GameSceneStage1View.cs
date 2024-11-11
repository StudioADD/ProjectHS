using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Define;
 
    public class GameSceneStage1View : ViewBase
    {
        [SerializeField]
        private Image progressing;

        [SerializeField]
        private Image leftImage;

        [SerializeField]
        private Image rightImage;

        [SerializeField]
        private Image[] items;

        private float progressingWidth;
        private RectTransform leftRectTransfrom;
        private RectTransform rightRectTransfrom;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            leftRectTransfrom = leftImage.GetComponent<RectTransform>();
            rightRectTransfrom = rightImage.GetComponent<RectTransform>();

            progressingWidth = progressing.GetComponent<RectTransform>().rect.width;

            return true;
        }

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
            ratio = Mathf.Clamp01(ratio);

            if (teamType == ETeamType.Left)
                progressing.fillAmount = ratio;

            leftRectTransfrom.anchoredPosition = new Vector3(progressingWidth * ratio, leftRectTransfrom.anchoredPosition.y);
        }

        public void UpdateRightProgressRatio(ETeamType teamType, float ratio)
        {
            ratio = Mathf.Clamp01(ratio);

            if (teamType == ETeamType.Right)
                progressing.fillAmount = ratio;

            rightRectTransfrom.anchoredPosition = new Vector3(progressingWidth * ratio, rightRectTransfrom.anchoredPosition.y);
        }
    }
