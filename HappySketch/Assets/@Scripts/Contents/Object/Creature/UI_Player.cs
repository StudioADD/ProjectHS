using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_Player : BaseObject
{
    private Animator animator;
    public override bool Init()
    {
        return base.Init();
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
    private void Reset()
    {
        animator = Util.GetOrAddComponent<Animator>(this.gameObject);
    }
}
