using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_ResultScene : UI_BaseScene
{
    [SerializeField]
    private RuntimeAnimatorController controller;

    [SerializeField]
    private GameObject player1;

    [SerializeField]
    private GameObject player2;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvasNotOverlay(gameObject, false);
        Managers.UI.SetSceneUI(this);

        return true;
    }

    public void SetInfo(ETeamType teamType)
    {
        // 동적생성!
        string prefabPath = $"{PrefabPath.OBJECT_PLAYER_PATH}/";
        switch (teamType)
        {
            case ETeamType.Left:
                prefabPath.Concat("TitleLeftPlayer");
                break;

            case ETeamType.Right:
                prefabPath.Concat("TitleRightPlayer");
                break;
        }

        GameObject player = Managers.Resource.Instantiate(prefabPath);
        Animator animator = player.GetComponent<Animator>();

        // 컨트롤러도 동적 생성 가능한가?
        animator.runtimeAnimatorController = controller;
    }
}

