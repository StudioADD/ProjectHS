using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MomDra
{
    public class SharkAvoidancePresenter : PresenterBase
    {
        public SharkAvoidancePresenter(ViewBase view) : base(view)
        {

        }

        /// <summary>
        /// 아이템 수를 설정하는 함수
        /// </summary>
        /// <param name="count"> 아이템 수: 0 ~ 3 </param>
        public void SetItemCount(int count)
        {
            if (view is GameSceneStage1View view1)
                view1.UpdateItemCount(count);
        }

        /// <summary>
        /// 총 이동경로 대비 현재 이동 경로 비율
        /// </summary>
        /// <param name="ratio">비율: 0 ~ 1 </param>
        public void SetProgressRatio(float ratio)
        {
            if (view is GameSceneStage1View view1)
                view1.UpdateProgressRatio(ratio);
        }
    }
}
