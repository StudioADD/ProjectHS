using System.Collections.Generic;
using System;

namespace Data
{
    #region JPlayerData
    [Serializable]
    public class JPlayerData
    {
        public int DataId; // StageNum
        public float MoveSpeed;

        // 2 Stage
        public float JumpPower;


        public int TeamType;
        public string TestName;
        public float moveSpeed;
        public float jumpForce;

        public float inputCooldown; // stage1 전진 쿨다운
        public float boosterTime; // stage1 부스터 시간
        public float hitInputIgnoreTime; // stage1 stage2 hit 시간
        public float hitBackDistance; // hit시 뒤로 밀려나는 거리
    }

    public class PlayerDataLoader : ILoader<int, JPlayerData>
    {
        public List<JPlayerData> Players = new List<JPlayerData>();

        public Dictionary<int, JPlayerData> MakeDict()
        {
            Dictionary<int, JPlayerData> PlayerDict = new Dictionary<int, JPlayerData>();
            foreach (JPlayerData test in Players)
                PlayerDict.Add(test.TeamType, test);
            return PlayerDict;
        }
    }
    #endregion

    #region JMonsterData
    [Serializable]
    public class JMonsterData
    {
        public int DataId;
        public string Name;
        public float MoveSpeed;
        public float SternTime;
    }

    public class MonsterDataLoader : ILoader<int, JMonsterData>
    {
        public List<JMonsterData> Monsters = new List<JMonsterData>();

        public Dictionary<int, JMonsterData> MakeDict()
        {
            Dictionary<int, JMonsterData> monsterDict = new Dictionary<int, JMonsterData>();
            foreach (JMonsterData monster in Monsters)
                monsterDict.Add(monster.DataId, monster);
            return monsterDict;
        }
    }
    #endregion
}