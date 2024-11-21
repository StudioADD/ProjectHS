using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextMeshProEffect : InitBase
{
    private Animator animator;
    private TextMeshProUGUI text;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        animator = GetComponent<Animator>();
        text = GetComponent<TextMeshProUGUI>();
        text.text = "";

        return true;
    }

    public void SetPosition(Vector3 position)
    {
        text.rectTransform.position = position;
    }

    public void OpenTextEffectUI(string str)
    {
        gameObject.SetActive(true);
        animator.SetTrigger("OnTrigger");
        text.text = str;
    }

    public void CloseTextEffectUI()
    {
        gameObject.SetActive(false);
    }
}
