using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class UI_ResultScene : UI_BaseScene
{
    [SerializeField]
    private RuntimeAnimatorController controller;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvasNotOverlay(gameObject, false);
        Managers.UI.SetSceneUI(this);

        return true;
    }

    public void SetInfo(Define.ETeamType teamType)
    {
        // 동적생성!
        string prefabPath = $"{PrefabPath.OBJECT_PLAYER_PATH}/";
        switch (teamType)
        {
            case Define.ETeamType.Left:
                prefabPath.Concat("TitleLeftPlayer");
                break;

            case Define.ETeamType.Right:
                prefabPath.Concat("TitleRightPlayer");
                break;
        }

        GameObject player = Managers.Resource.Instantiate(prefabPath);
        Animator animator = player.GetComponent<Animator>();

        // 컨트롤러도 동적 생성 가능한가?
        animator.runtimeAnimatorController = controller;
    }
}

