using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_ResultScene : UI_BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvasNotOverlay(gameObject, false);
        Managers.UI.SetSceneUI(this);

        SetInfo(Managers.Game.GetGameWinnerTeam());

        ChangeSceneAfterTime(EScene.TitleScene, 5f);

        return true;
    }

    private void SetInfo(ETeamType teamType)
    {
        string prefabPath = $"{PrefabPath.OBJECT_PLAYER_PATH}";
        switch (teamType)
        {
            case ETeamType.Left:
                prefabPath = $"{prefabPath}/UILeftPlayer";
                break;

            case ETeamType.Right:
                prefabPath = $"{prefabPath}/UIRightPlayer";
                break;
        }

        GameObject player = Managers.Resource.Instantiate(prefabPath);
        player.transform.position = Vector3.up * -1.15f;
        player.GetComponent<UI_Player>().SetInfo(false);
    }
}