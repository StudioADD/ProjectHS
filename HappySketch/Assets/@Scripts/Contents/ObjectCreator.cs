using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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

    public static T SpawnItem<T>(ItemParam param, Vector3 spawnPosition) where T : BaseItem
    {
        string name = typeof(T).Name;
        BaseItem item = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_ITEM_PATH}/{name}").GetComponent<BaseItem>();

        if (item == null)
        {
            Debug.LogWarning($"아이템 스폰 실패 : {name}");
            return null;
        }

        item.transform.parent = ItemRoot.transform.transform;
        item.transform.localPosition = spawnPosition;
        item.SetInfo(param);
        return item as T;
    }

    public static T SpawnEffect<T>(EEffectType effectType, Vector3 spawnPosition) where T : BaseEffectObject
    {
        string name = Util.EnumToString(effectType);
        BaseEffectObject effect = Managers.Resource.Instantiate($"{PrefabPath.OBJECT_EFFECT_PATH}/{name}").GetComponent<BaseEffectObject>();

        if (effect == null)
        {
            Debug.LogWarning($"이펙트 스폰 실패 : {name}");
            return null;
        }

        effect.transform.parent = EffectRoot.transform.transform;
        effect.transform.localPosition = spawnPosition;
        effect.SetInfo(); // 인자 생기면 EffectParam 고려
        return effect as T;
    }
}
