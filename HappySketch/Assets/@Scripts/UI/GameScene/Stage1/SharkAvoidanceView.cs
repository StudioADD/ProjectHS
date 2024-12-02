using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class SharkAvoidanceView : ViewBase
{
    // Progress
    [SerializeField]
    private Image progressing;

    [SerializeField]
    private Image leftProgressingImage;

    [SerializeField]
    private Image rightProgressingImage;

    // Booster
    [SerializeField]
    private Image activeBoosterImage;

    [SerializeField]
    private Image activeBoosterEffect;

    [SerializeField]
    private Image inactiveBoosterImage;

    // Item
    [SerializeField]
    private Image[] activeItems;

    [SerializeField]
    private Image[] inactiveItems;

    private float progressingWidth;
    private RectTransform leftRectTransfrom;
    private RectTransform rightRectTransfrom;

    private float leftImgWidth;
    private float rightImgWidth;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        leftRectTransfrom = leftProgressingImage.GetComponent<RectTransform>();
        rightRectTransfrom = rightProgressingImage.GetComponent<RectTransform>();

        progressingWidth = progressing.GetComponent<RectTransform>().rect.width;

        leftImgWidth = leftProgressingImage.GetComponent<RectTransform>().rect.width;
        rightImgWidth = rightProgressingImage.GetComponent<RectTransform>().rect.width;

        SetActiveActiveItemsImage(false);
        SetActiveInActiveItemsImage(false);
        UpdateLeftRatio(0f);
        UpdateRightRatio(0f);
        UpdateProgressingBarRatio(0f);
        UpdateItemCount(0);

        return true;
    }

    public void UpdateItemCount(int count)
    {
        if (count == 0)
        {
            activeBoosterEffect.gameObject.SetActive(false);
            SetActiveInActiveItemsImage(false);
            SetActiveActiveItemsImage(false);
        }
        else if (count == 3)
        {
            activeBoosterEffect.gameObject.SetActive(true);
            SetActiveInActiveItemsImage(false);
            SetActiveActiveItemsImage(true);
        }
        else
        {
            // 노란색 아이템 활성화
            for (int i = 0; i < count; ++i)
            {
                inactiveItems[i].gameObject.SetActive(true);
            }
        }
    }

    public void UpdateItemRatio(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);

        activeBoosterImage.fillAmount = ratio;
    }

    public void UpdateLeftRatio(float ratio)
    {
        leftRectTransfrom.anchoredPosition = new Vector3(progressingWidth * ratio - leftImgWidth / 2.2f, leftRectTransfrom.anchoredPosition.y);
    }

    public void UpdateRightRatio(float ratio)
    {
        rightRectTransfrom.anchoredPosition = new Vector3(progressingWidth * ratio - rightImgWidth / 2.2f, rightRectTransfrom.anchoredPosition.y);
    }

    public void UpdateProgressingBarRatio(float ratio)
    {
        progressing.fillAmount = ratio;
    }

    private void SetActiveInActiveItemsImage(bool isActive)
    {
        foreach (Image image in inactiveItems)
        {
            image.gameObject.SetActive(isActive);
        }
    }

    private void SetActiveActiveItemsImage(bool isActive)
    {
        foreach (Image image in activeItems)
        {
            image.gameObject.SetActive(isActive);
        }
    }
}
