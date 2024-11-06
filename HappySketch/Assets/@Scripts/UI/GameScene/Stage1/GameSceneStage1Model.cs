using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MomDra
{
    public class GameSceneStage1Model : GameSceneModelBase
    {
        public UnityEvent<float> ProgressEvent;
        public UnityEvent<int> ItemCountEvent;
    }
}
