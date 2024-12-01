using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class UIParam { }

#region PopupUI Param
public class UIFadeEffectParam : UIParam
{
    public Func<bool> fadeInEffectCondition;
    public Action onFadeOutCallBack;
    public Action onFadeInCallBack;

    public UIFadeEffectParam(Func<bool> fadeInEffectCondition = null, Action onFadeOutCallBack = null, Action onFadeInCallBack = null)
    {
        this.fadeInEffectCondition = fadeInEffectCondition;
        this.onFadeOutCallBack = onFadeOutCallBack;
        this.onFadeInCallBack = onFadeInCallBack;
    }
}
#endregion

#region ObjectUI Param
public class UIGameStartCounterParam : UIParam
{
    public int Time { get; private set; }
    public Action OnEndCount { get; private set; }

    public UIGameStartCounterParam(int time, Action onEndCount = null)
    {
        this.Time = time;
        this.OnEndCount = onEndCount;
    }
}

public class UIWinLoseParam : UIParam
{
    public struct WinCount
    {
        private ETeamType teamType;
        private int winCount;

        public WinCount(ETeamType teamType, int winCount)
        {
            this.teamType = teamType;
            this.winCount = winCount;
        }
    }

    public ETeamType WinTeam { get; private set; }

    public WinCount LeftWinCount { get; private set; }
    public WinCount RightWinCount { get; private set; }

    public UIWinLoseParam(ETeamType winTeam, WinCount leftWinCount, WinCount rightWinCount)
    {
        WinTeam = winTeam;
        LeftWinCount = leftWinCount;
        RightWinCount = rightWinCount;
    }
}
#endregion