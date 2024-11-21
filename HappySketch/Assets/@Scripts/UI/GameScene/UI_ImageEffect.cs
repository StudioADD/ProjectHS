using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ImageEffect : InitBase
{
    Animator animator;
    Image image;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        animator = GetComponent<Animator>();
        image = GetComponent<Image>();

        return true;
    }

    public void OpenImageEffectUI()
    {
        animator.SetTrigger("OnTrigger");
    }
}
