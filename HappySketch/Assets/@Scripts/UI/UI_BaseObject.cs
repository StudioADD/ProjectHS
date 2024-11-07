using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BaseObject : InitBase
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;



        return true;
    }

    public virtual void SetInfo(UIParam param = null) { }

    public virtual void OpenObjectUI()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void CloseObjectUI()
    {
        this.gameObject.SetActive(false);
    }
}
