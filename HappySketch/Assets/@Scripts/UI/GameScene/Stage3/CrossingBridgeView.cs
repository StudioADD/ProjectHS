using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossingBridgeView : ViewBase
{
    [SerializeField]
    private Image[] images;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Color transparentColor = Color.white;
        transparentColor.a = 0.5f;

        SetColor(transparentColor);

        return true;
    }

    public void SetColor(Color color)
    {
        foreach (Image image in images)
            image.color = color;
    }

    public Color GetColor()
    {
        return images[0].color;
    }
}
