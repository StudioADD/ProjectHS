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

    [SerializeField, ReadOnly]
    private Image colockImg;

    private void Reset()
    {
        time = Util.FindChild<TextMeshProUGUI>(gameObject, "Text_Timer", true);
        score = Util.FindChild<TextMeshProUGUI>(gameObject, "Text_Score_Int", true);

        itemCounts = new TextMeshProUGUI[3];
        itemCounts[0] = Util.FindChild<TextMeshProUGUI>(gameObject, "Text_Red", true);
        itemCounts[1] = Util.FindChild<TextMeshProUGUI>(gameObject, "Text_Green", true);
        itemCounts[2] = Util.FindChild<TextMeshProUGUI>(gameObject, "Text_Blue", true);

        colockImg = Util.FindChild<Image>(gameObject, "Timer_Front", true);
    }

    public void UpdateTime(string time)
    {
        this.time.text = time;
    }

    public void UpdateTimeRatio(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        colockImg.fillAmount = ratio;
    }

    public void UpdateScore(int score)
    {
        this.score.text = score.ToString();
    }

    public void UpdateItemCount(int[] itemCounts)
    {
        for (int i = 0; i < this.itemCounts.Length; ++i)
        {
            this.itemCounts[i].text = itemCounts[i].ToString();
        }
    }
}
