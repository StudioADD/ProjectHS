using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Updown : MonoBehaviour
{
    [SerializeField]
    private float floatAmplitude = 45f; // 상하로 움직이는 최대 높이

    [SerializeField]
    private float floatFrequency = 1f;  // 움직임의 주기 (속도)

    private Vector2 startPosition; // 초기 위치 저장

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    private void Start()
    {
        StartCoroutine(UpDownCoroutine());
    }

    private IEnumerator UpDownCoroutine()
    {
        while (true)
        {
            float elapsedTime = 0f;

            while (elapsedTime < Mathf.PI * 2)
            {
                elapsedTime += Time.deltaTime * floatFrequency;
                float offsetY = Mathf.Sin(elapsedTime) * floatAmplitude; // Sin 함수를 사용한 상하 이동 계산
                rectTransform.anchoredPosition = startPosition + new Vector2(0, offsetY); // Y축으로 이동 적용
                yield return null; // 다음 프레임까지 대기
            }
        }
    }
}
