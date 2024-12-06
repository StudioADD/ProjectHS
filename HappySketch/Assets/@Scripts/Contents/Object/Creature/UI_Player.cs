using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Player : BaseObject
{
    private Animator animator;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        animator = Util.GetOrAddComponent<Animator>(gameObject);

        return true;
    }

    public void SetInfo(bool isTitle)
    {
        if(isTitle)
        {
            animator.SetTrigger("Title_Trigger");
        }
        else 
        {
            animator.SetTrigger("Victoty_Trigger");
        }
    }
}
