using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossingBridgeView : ViewBase
{
    [SerializeField, ReadOnly]
    private Image[] goggleImages;

    [SerializeField, ReadOnly]
    private Image arrowImage;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Color transparentColor = Color.white;
        transparentColor.a = 0.5f;

        SetColor(transparentColor);

        return true;
    }

    private void Reset()
    {
        goggleImages = Util.FindChild<Transform>(gameObject, "Goggle_icon").GetComponentsInChildren<Image>();
        arrowImage = Util.FindChild<Image>(gameObject, "Img_Arrow");
    }

    public void SetColor(Color color)
    {
        foreach (Image image in goggleImages)
            image.color = color;
    }

    public Color GetColor()
    {
        return goggleImages[0].color;
    }
}
