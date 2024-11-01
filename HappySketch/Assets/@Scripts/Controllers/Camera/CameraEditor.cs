using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEditor : InitBase
{
    [SerializeField] TeamCamera cam;
    [SerializeField] BaseObject testTarget;
    [SerializeField] Vector3 deltaPosVec;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    private void Start()
    {
        cam.SetTarget(testTarget);
    }

    private void Update()
    {
        cam.SetInfo(deltaPosVec);
    }
}
