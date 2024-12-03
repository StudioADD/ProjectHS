using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollectingCandy
{
    public class ScoreCollector : InitBase
    {
        [SerializeField, ReadOnly] int leftTotalScore = 0;
        [SerializeField, ReadOnly] int rightTotalScore = 0;

        [SerializeField, ReadOnly] bool isSetLeftScore = false;
        [SerializeField, ReadOnly] bool isSetRightScore = false;

        // 여기서 UI에 콜백 연결을 하는 게 좋아보이긴 함 ㅇㅇ;;


    }
}
