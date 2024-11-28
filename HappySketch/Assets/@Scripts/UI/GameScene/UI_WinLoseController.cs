using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_WinLoseController : MonoBehaviour
{
    [SerializeField]
    private UI_WinLose leftWinLose;

    [SerializeField]
    private UI_WinLose rightWinLose;

    public void EndStage(ETeamType teamType, Action onEnd)
    {
        switch(teamType)
        {
            case ETeamType.Left:
                leftWinLose.Win();
                rightWinLose.Lose();
                break;

            case ETeamType.Right:
                leftWinLose.Lose();
                rightWinLose.Win();
                break;
        }
    }
}  
