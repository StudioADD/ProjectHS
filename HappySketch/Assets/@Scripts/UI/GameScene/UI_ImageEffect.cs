using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ImageEffect : InitBase
{
    private Animator animator;
    private Image image;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        animator = GetComponent<Animator>();
        image = GetComponent<Image>();

        return true;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void OpenImageEffectUI()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("OnTrigger");
    }

    public void CloseTextEffectUI()
    {
        gameObject.SetActive(false);
    }
}
 