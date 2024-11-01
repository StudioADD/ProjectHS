using Data;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataMgr
{
    public Dictionary<int, JTestData> TestDict { get; private set; } = new Dictionary<int, JTestData>();
    public Dictionary<int, JMonsterData> MonsterDict { get; private set; } = new Dictionary<int, JMonsterData>();

    public void Init()
    {
        TestDict = LoadJson<TestDataLoader, int, JTestData>("TestData").MakeDict();
        MonsterDict = LoadJson<MonsterDataLoader, int, JMonsterData>("MonsterData").MakeDict();
    } 

    public Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    Loader LoadJson<Loader>(string path) where Loader : class
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}