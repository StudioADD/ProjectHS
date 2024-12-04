using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace CollectingCandy
{
    public class ScoreCollector : InitBase
    {
        [SerializeField, ReadOnly] CollectingCandyParam leftStageParam = null;
        [SerializeField, ReadOnly] CollectingCandyParam rightStageParam = null;

        [SerializeField, ReadOnly] bool isSetLeftScore = false;
        [SerializeField, ReadOnly] bool isSetRightScore = false;
        
        public event Action<ETeamType> OnEndGameCallBack;
        public event Action OnGameTimerEnd;

        public void StartStage()
        {
            if (Managers.UI.SceneUI is UI_GameScene uiGameScene &&
                uiGameScene.GetStageUI() is CollectCandyModel uiCollectCandy)
            {
                uiCollectCandy.StartTimer(90, OnGameTimerEnd);
            }
        }

        public void SetTotalScore(CollectingCandyParam param)
        {
            if (coCollectTotalScore == null)
                coCollectTotalScore = StartCoroutine(CoCollectTotalScore());

            if (param.TeamType == ETeamType.Left)
            {
                leftStageParam = param;
                isSetLeftScore = true;
            }
            else if (param.TeamType == ETeamType.Right)
            {
                rightStageParam = param;
                isSetRightScore = true;
            }
        }

        Coroutine coCollectTotalScore = null;
        private IEnumerator CoCollectTotalScore()
        {
            yield return new WaitUntil(() => isSetLeftScore && isSetRightScore);

            ETeamType winnerTeam = ETeamType.Left; // 승리 조건 판단해야 함


            OnEndGameCallBack?.Invoke(winnerTeam);
            coCollectTotalScore = null;
        }
    }
}
