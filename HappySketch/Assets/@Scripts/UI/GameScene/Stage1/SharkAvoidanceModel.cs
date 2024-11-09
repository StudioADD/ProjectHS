using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Define;

namespace MomDra
{
    public class SharkAvoidanceModel : ModelBase
    {
        public ETeamType TeamType { get; }
        private int itemCount;
        private float leftProgressRatio;
        private float rightProgressRatio;

        public SharkAvoidanceModel(ETeamType teamType)
        {
            TeamType = teamType;
        }

        public void SetItemCount(int itemCount)
        {
            this.itemCount = itemCount;
        }

        public void SetLeftProgressRatio(float ratio)
        {
            leftProgressRatio = ratio;
        }

        public void SetRightProgressRatio(float ratio)
        {
            rightProgressRatio = ratio;
        }
    }
}
