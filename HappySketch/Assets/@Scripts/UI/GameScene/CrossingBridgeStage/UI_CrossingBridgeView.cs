using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CrossingBridgeView : UI_ViewBase
{
    [SerializeField, ReadOnly]
    private Image[] goggleImages;

    [SerializeField]
    private Image effect;


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
        effect = Util.FindChild<Image>(gameObject, "goggle_activate_effect", true);
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

    public void RotateEffect()
    {
        effect.gameObject.SetActive(true);
    }

    public void StopEffect()
    {
        effect.gameObject.SetActive(false);
    }
}
