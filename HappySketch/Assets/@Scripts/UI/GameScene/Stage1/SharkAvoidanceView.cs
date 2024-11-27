using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class SharkAvoidanceView : ViewBase
{
    [SerializeField]
    private Image progressing;

    [SerializeField]
    private Image leftImage;

    [SerializeField]
    private Image rightImage;

    [SerializeField]
    private Image[] activeBoosterImage;

    [SerializeField]
    private Image inactiveBoosterImage;

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

        leftRectTransfrom = leftImage.GetComponent<RectTransform>();
        rightRectTransfrom = rightImage.GetComponent<RectTransform>();

        progressingWidth = progressing.GetComponent<RectTransform>().rect.width;

        leftImgWidth = leftImage.GetComponent<RectTransform>().rect.width;
        rightImgWidth = rightImage.GetComponent<RectTransform>().rect.width;

        DeActivateBoosterImage();
        SetActiveActiveItemsImage(false);
        SetActiveInActiveItemsImage(false);
        UpdateLeftProgressRatio(0f);
        UpdateRightProgressRatio(0f);
        UpdateProgressingBar(0f);

        return true;
    }

    public void UpdateItemCount(int count)
    {
        Debug.Log($"아이템 개수: {count}");

        if (count == 0)
        {
            DeActivateBoosterImage();
            SetActiveInActiveItemsImage(false);
            SetActiveActiveItemsImage(false);
        }
        else if (count == 3)
        {
            ActivateBoosterImage();
            SetActiveInActiveItemsImage(false);
            SetActiveActiveItemsImage(true);
        }
        else
        {
            for (int i = 0; i < count; ++i)
            {
                inactiveItems[i].gameObject.SetActive(true);
            }
        }
    }

    public void UpdateLeftProgressRatio(float ratio)
    {
        Debug.Log($"hahahah : {ratio}");
        leftRectTransfrom.anchoredPosition = new Vector3(progressingWidth * ratio - leftImgWidth / 2.2f, leftRectTransfrom.anchoredPosition.y);
    }

    public void UpdateRightProgressRatio(float ratio)
    {
        rightRectTransfrom.anchoredPosition = new Vector3(progressingWidth * ratio - rightImgWidth / 2.2f, rightRectTransfrom.anchoredPosition.y);
    }

    public void UpdateProgressingBar(float ratio)
    {
        progressing.fillAmount = ratio;
    }

    private void ActivateBoosterImage()
    {
        foreach(Image image in activeBoosterImage)
        {
            image.gameObject.SetActive(true);
        }

        inactiveBoosterImage.gameObject.SetActive(false);
    }

    private void DeActivateBoosterImage()
    {
        foreach(Image image in activeBoosterImage)
        {
            image.gameObject.SetActive(false);
        }

        inactiveBoosterImage.gameObject.SetActive(true);
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

    public float GetProgressRatio()
    {
        return progressing.fillAmount;
    }
}
