using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScene : BaseScene
{
    [SerializeField] GameObject fireWork;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(CoWaitFActiveireWork(2));
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.ResultScene;

        return true;
    }

    private IEnumerator CoWaitFActiveireWork(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        ActiveFireWorkObject();
    }

    private void ActiveFireWorkObject()
    {
        Managers.Sound.PlaySfx(Define.ESfxSoundType.UI_EndGame);
        fireWork?.gameObject.SetActive(true);
    }

    public override void Clear()
    {

    }
}