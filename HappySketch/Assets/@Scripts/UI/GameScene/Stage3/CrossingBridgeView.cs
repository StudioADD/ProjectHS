using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossingBridgeView : ViewBase
{
    [SerializeField]
    private Image image;


    public void SetActiveGoogleImage(bool isActive)
    {
        image.gameObject.SetActive(isActive);
    }
}
