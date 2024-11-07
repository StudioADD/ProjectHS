using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Data
{
    [Serializable]
    public class JPlayerData
    {
        public int DataId; // 좌 0 우 1 
        public string TestName;
        public float moveSpeed;
        public float jumpForce;

        public float inputCooldown; // stage1 전진 쿨다운
        public float boosterTime; // stage1 부스터 시간
        public float hitInputIgnoreTime; // stage1 stage2 hit 시간
        public float hitBackDistance; // hit시 뒤로 밀려나는 거리
    }

    public class PlayerData : ILoader<int, JPlayerData>
    {
        public List<JPlayerData> Tests = new List<JPlayerData>();

        public Dictionary<int, JPlayerData> MakeDict()
        {
            Dictionary<int, JPlayerData> testDict = new Dictionary<int, JPlayerData>();
            foreach (JPlayerData test in Tests)
                testDict.Add(test.DataId, test);
            return testDict;
        }
    }
}