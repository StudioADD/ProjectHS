using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MomDra
{
    public class GameScenePresenter
    {
        private GameSceneView view;

        // 내 모델로 테스트 해보자
        //private GameSceneModel model;

        public GameScenePresenter(GameSceneView view)
        {
            this.view = view;
            // 여기서 모델의 이벤트 등록이 필요하다
            // 모델에서는 이벤트를 발생 시켜야 한다

            view.SetGameScenePresenter(this);
        }

        /// <summary>
        /// 아이템 수를 설정하는 함수
        /// </summary>
        /// <param name="count"> 아이템 수: 0 ~ 3 </param>
        public void SetItemCount(int count)
        {
            view.UpdateItemCount(count);
        }

        /// <summary>
        /// 총 이동경로 대비 현재 이동 경로 비율
        /// </summary>
        /// <param name="ratio">비율: 0 ~ 1 </param>
        public void SetProgressRatio(float ratio)
        {
            view.UpdateProgressRatio(ratio);
        }
    }
}
