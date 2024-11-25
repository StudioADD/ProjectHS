using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterCreater
{
    #region Root
    private static GameObject _monsterRoot;
    public static GameObject MonsterRoot
    {
        get
        {
            if (_monsterRoot == null) _monsterRoot = GameObject.Find("@MonsterRoot");
            if (_monsterRoot == null) _monsterRoot = new GameObject { name = "@MonsterRoot" };
            return _monsterRoot;
        }
    }
    #endregion

    public static T SpawnMonster<T>() where T : BaseMonster
    {
        string name = typeof(T).Name;
        BaseMonster monster = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_MONSTER_PATH}/{name}").GetComponent<BaseMonster>();

        if(monster == null)
        {
            Debug.LogWarning($"몬스터 스폰 실패 : {name}");
            return null;
        }

        monster.transform.parent = MonsterRoot.transform.transform;
        return monster as T;
    }
}
