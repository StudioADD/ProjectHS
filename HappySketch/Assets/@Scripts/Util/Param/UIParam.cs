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
    public ETeamType WinTeam { get; private set; }
    public int LeftWinCount { get; private set; }
    public int RightWinCount { get; private set; }
    public Action onEnd { get; private set; }

    public UIWinLoseParam(ETeamType winTeam, int leftWinCount, int rightWinCount, Action onEnd)
    {
        WinTeam = winTeam;
        LeftWinCount = leftWinCount;
        RightWinCount = rightWinCount;
    }
}

public class UIScorePoolParam : UIParam
{
    public Camera leftCamera;
    public Camera rightCamera;

    public UIScorePoolParam(Camera leftCamera, Camera rightCamera)
    {
        this.leftCamera = leftCamera;
        this.rightCamera = rightCamera;
    }
}
#endregion