using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossingBridge
{
    public class PlayerPointGroup : InitBase
    {
        [field: SerializeField, ReadOnly] public Transform PlayerStartPoint { get; private set; }
        [field: SerializeField, ReadOnly] public Transform PlayerSavePoint { get; private set; }
        [field: SerializeField, ReadOnly] public Transform PlayerEndPoint { get; private set; }

        private void Reset()
        {
            PlayerStartPoint = Util.FindChild<Transform>(this.gameObject, "PlayerStartPoint");
            PlayerSavePoint = Util.FindChild<Transform>(this.gameObject, "PlayerSavePoint");
            PlayerEndPoint = Util.FindChild<Transform>(this.gameObject, "PlayerEndPoint");
        }
    }
}