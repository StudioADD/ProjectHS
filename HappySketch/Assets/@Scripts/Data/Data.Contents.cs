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


    }

    public class PlayerDataLoader : ILoader<int, JPlayerData>
    {
        public List<JPlayerData> Players = new List<JPlayerData>();

        public Dictionary<int, JPlayerData> MakeDict()
        {
            Dictionary<int, JPlayerData> PlayerDict = new Dictionary<int, JPlayerData>();
            foreach (JPlayerData player in Players)
                PlayerDict.Add(player.DataId, player);
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