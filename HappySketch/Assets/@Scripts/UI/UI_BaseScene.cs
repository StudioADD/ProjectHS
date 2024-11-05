using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_BaseScene : InitBase
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, false);
        Managers.UI.SetSceneUI(this);
        return true;
    }
    
    protected void ChangeSceneAfterTime(Define.EScene scene, float time)
    {
        StartCoroutine(WaitAndChangeScene(scene, time));
    }

    protected IEnumerator WaitAndChangeScene(Define.EScene scene, float time)
    {
        yield return new WaitForSeconds(time);

        Managers.Scene.LoadScene(scene);
    }
}
