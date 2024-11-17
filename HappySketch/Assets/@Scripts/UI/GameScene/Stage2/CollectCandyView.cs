using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using static Define;

public class CollectCandyView : ViewBase
{
    [SerializeField]
    private TextMeshProUGUI time;

    [SerializeField]
    private TextMeshProUGUI score;

    [SerializeField]
    private TextMeshProUGUI[] itemCount;

    public void UpdateTime(string time)
    {
        this.time.text = time;
    }

    public void UpdateScore(int score)
    {
        this.score.text = score.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemType"></param>
    public void UpdateItemCount(EItemType itemType, int itemCount)
    {
        if (itemType < 0 && (int)itemType >= this.itemCount.Length)
            throw new ArgumentOutOfRangeException($"{itemCount}");

        this.itemCount[(int)itemType].text = itemCount.ToString();
    }
}
