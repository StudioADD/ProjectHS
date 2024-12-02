using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace CrossingBridge
{
    public enum EPlatformType
    {
        StartPoint = 0,
        SavePoint = 7,
        EndPoint = 14,

        Normal = 16
    }

    public class PlatformGroupController : InitBase
    {
        [field: SerializeField, ReadOnly]
        List<BasePlatformGroup> platformGroupList = new List<BasePlatformGroup>();

        private void Reset()
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if(child.TryGetComponent(out BasePlatformGroup platformGroup))
                    platformGroupList.Add(platformGroup);
            }
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            return true;
        }

        public void SetInfo(Action<int, ETeamType> onLandPlayer)
        {
            for(int i = 0; i < platformGroupList.Count; i++)
            {
                platformGroupList[i].SetInfo(onLandPlayer, i);
            }
        }

        public Vector3 GetPlatformPos(int id, ETeamType teamType, EDirection dir = EDirection.Left)
        {
            if (0 > id || id >= platformGroupList.Count)
            {
#if DEBUG
                Debug.LogWarning($"범위 벗어남! 벗어난 ID : {id}");
#endif
                return Vector3.zero;
            }

            return platformGroupList[id].GetPlatformPosition(teamType, dir);
        }
    }
}
