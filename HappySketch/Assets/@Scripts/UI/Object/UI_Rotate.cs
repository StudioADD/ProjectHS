using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Rotate : MonoBehaviour
{
    [SerializeField]
    private float rotSpeed;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        while(true)
        {
            rectTransform.Rotate(new Vector3(0f, 0f, -rotSpeed * Time.deltaTime));

            yield return null;
        }
    }
}
