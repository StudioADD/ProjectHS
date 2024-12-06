using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ScoreText : UI_BaseObject
{
    private TextMeshProUGUI scoreText;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        scoreText = GetComponent<TextMeshProUGUI>();
        scoreText.text = "";

        return true;
    }

    public override void SetInfo(UIParam param = null)
    {
        base.SetInfo(param);

        if(param is UIScoreTextParam scoreTextParam)
        {
            scoreText.text = scoreTextParam.Score.ToString();
            transform.position = scoreTextParam.spawnPoint;
            scoreText.color = scoreTextParam.textColor;
        }
    }

    public override void OpenObjectUI()
    {
        base.OpenObjectUI();

        if(coScoreTextEffect == null)
            coScoreTextEffect = StartCoroutine(CoScoreTextEffect(0.5f));
    }

    Coroutine coScoreTextEffect = null;
    private IEnumerator CoScoreTextEffect(float effectTime)
    {
        Color tempColor = scoreText.color;
        tempColor.a = 1f;
        while (tempColor.a > 0.01f)
        {
            tempColor.a -= Time.deltaTime / effectTime;
            scoreText.color = tempColor;
            this.transform.position += new Vector3(0, Time.deltaTime * 150f, 0);

            yield return null;
        }

        Managers.Resource.Destroy(gameObject);
        coScoreTextEffect = null;
    }
}
