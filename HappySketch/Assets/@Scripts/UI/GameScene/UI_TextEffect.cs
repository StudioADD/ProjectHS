using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextEffect : InitBase
{
    Animator animator;
    Text text;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        animator = GetComponent<Animator>();
        text = GetComponent<Text>();
        text.text = "";

        return true;
    }

    public void OpenTextEffectUI(string str)
    {
        animator.SetTrigger("OnTrigger");
        text.text = str;
    }
}
