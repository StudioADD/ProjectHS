using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Updown : MonoBehaviour
{
    [SerializeField]
    private float changeDirTime;

    [SerializeField]
    private float moveSpeed;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartCoroutine(UpDownCoroutine());
    }

    private IEnumerator UpDownCoroutine()
    {
        int dir = 1;
        float time = 0f;

        Vector2 pos = rectTransform.anchoredPosition;

        while (true)
        {
            time += Time.deltaTime;
            if (time >= changeDirTime)
            {
                time = 0f;
                dir = -dir;
            }

            pos.y += dir * moveSpeed * Time.deltaTime;

            rectTransform.anchoredPosition = pos;

            yield return null;
        }
    }
}
