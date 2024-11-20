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
    private Image boosterImage;

    [SerializeField]
    private Image[] items;

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

        return true;
    }

    public void UpdateItemCount(int count)
    {
        Debug.Log($"아이템 개수: {count}");

        if (count == 0) DeActivateBoosterImage();
        else if (count == 3) ActivateBoosterImage();

        for (int i = 0; i < count; ++i)
        {
            items[i].gameObject.SetActive(true);
        }

        for (int i = count; i < items.Length; ++i)
        {
            items[i].gameObject.SetActive(false);
        }
    }

    public void UpdateLeftProgressRatio(float ratio)
    {
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
        boosterImage.color = Color.white;
    }

    private void DeActivateBoosterImage()
    {
        boosterImage.color = Color.gray;
    }
}
