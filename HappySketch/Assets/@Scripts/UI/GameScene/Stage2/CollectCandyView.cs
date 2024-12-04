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

    public void SetUIScore(Vector3 pos, int score)
    {

    }

    public void UpdateItemCount(int[] itemCounts)
    {
        for(int i = 0; i < itemCount.Length; ++i)
        {
            itemCount[i].text = itemCounts[i].ToString();
        }
    }
}
