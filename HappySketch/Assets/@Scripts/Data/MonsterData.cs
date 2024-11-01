using System.Collections.Generic;
using System;

namespace Data
{
    [Serializable]
    public class JMonsterData
    {
        public int DataId;
        public string Name;
        public int MoveSpeed;
        public int Damage;
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
}