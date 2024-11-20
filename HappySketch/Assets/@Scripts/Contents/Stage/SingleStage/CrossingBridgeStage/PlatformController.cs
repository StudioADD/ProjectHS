using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace CrossingBridge
{
    public class PlatformController : InitBase
    {
        [field: SerializeField, ReadOnly]
        List<PlatformGroup> platformGroupList = new List<PlatformGroup>();
        readonly float offSetPosY = 3.0f;

        private void Reset()
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if(child.TryGetComponent(out PlatformGroup platformGroup))
                    platformGroupList.Add(platformGroup);
            }
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            return true;
        }

        public void SetInfo()
        {
            for(int i = 0; i < platformGroupList.Count; i++)
            {
                platformGroupList[i].SetInfo(OnLandPlayerCallBack, i);
            }
        }

        public void OnLandPlayerCallBack(int platformId, ETeamType teamType, bool isLandable)
        {

        }

        public Vector3 GetPlatformPos(int id)
        {
            if (0 > id || id >= platformGroupList.Count)
            {
                Debug.LogWarning($"범위 벗어남! ID : {id}");
                return Vector3.zero;
            }

            return platformGroupList[id].transform.position + new Vector3(0, offSetPosY, 0);
        }
    }

}
