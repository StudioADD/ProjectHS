using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using static Define;

public class CollectCandyView : ViewBase
{
    [SerializeField, ReadOnly]
    private TextMeshProUGUI time;

    [SerializeField, ReadOnly]
    private TextMeshProUGUI score;

    [SerializeField, ReadOnly]
    private TextMeshProUGUI[] itemCounts;

    // Pool?
    [SerializeField, ReadOnly]
    private TextMeshProUGUI[] scorePool;
    private int poolIndex = 0;

    private void Reset()
    {
        time = Util.FindChild<TextMeshProUGUI>(gameObject, "Text_Timer", true);
        score = Util.FindChild<TextMeshProUGUI>(gameObject, "Text_Score", true);

        itemCounts = new TextMeshProUGUI[3];
        itemCounts[0] = Util.FindChild<TextMeshProUGUI>(gameObject, "Text_Red", true);
        itemCounts[1] = Util.FindChild<TextMeshProUGUI>(gameObject, "Text_Green", true);
        itemCounts[2] = Util.FindChild<TextMeshProUGUI>(gameObject, "Text_Blue", true);

        scorePool = Util.FindChild<Transform>(gameObject, "TextPool", true).GetComponentsInChildren<TextMeshProUGUI>(true);
    }

    public void UpdateTime(string time)
    {
        this.time.text = time;
    }

    public void UpdateScore(int score)
    {
        this.score.text = score.ToString();
    }

    public void UpdateUIScore(Vector3 pos, int score)
    {
        scorePool[poolIndex].gameObject.SetActive(true);
        scorePool[poolIndex].text = score.ToString();
        scorePool[poolIndex].rectTransform.position = pos;

        poolIndex = (poolIndex + 1) % scorePool.Length;
    }

    public void UpdateItemCount(int[] itemCounts)
    {
        for (int i = 0; i < this.itemCounts.Length; ++i)
        {
            this.itemCounts[i].text = itemCounts[i].ToString();
        }
    }
}
