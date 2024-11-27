using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectCreator
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

    private static GameObject _itemRoot;
    public static GameObject ItemRoot
    {
        get
        {
            if (_itemRoot == null) _itemRoot = GameObject.Find("@ItemRoot");
            if (_itemRoot == null) _itemRoot = new GameObject { name = "@ItemRoot" };
            return _itemRoot;
        }
    }

    private static GameObject _effectRoot;
    public static GameObject EffectRoot
    {
        get
        {
            if (_effectRoot == null) _effectRoot = GameObject.Find("@EffectRoot");
            if (_effectRoot == null) _effectRoot = new GameObject { name = "@EffectRoot" };
            return _effectRoot;
        }
    }
    #endregion

    public static T SpawnMonster<T>(EMonsterType monsterType, Vector3 spawnPosition) where T : BaseMonster
    {
        string name = Util.EnumToString(monsterType);
        BaseMonster monster = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_MONSTER_PATH}/{name}").GetComponent<BaseMonster>();

        if(monster == null)
        {
            Debug.LogWarning($"몬스터 스폰 실패 : {name}");
            return null;
        }

        monster.transform.parent = MonsterRoot.transform.transform;
        monster.transform.localPosition = spawnPosition;
        monster.SetInfo((int)monsterType);
        return monster as T;
    }

    public static T SpawnItem<T>(EItemType itemType, Vector3 spawnPosition) where T : BaseItem
    {
        string name = Util.EnumToString(itemType);
        BaseItem item = null;



        return item as T;
    }
}
