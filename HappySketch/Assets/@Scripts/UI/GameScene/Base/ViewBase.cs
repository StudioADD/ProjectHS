using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MomDra
{
    public abstract class ViewBase : MonoBehaviour
    {
        protected PresenterBase presenter;

        public void SetPresenter(PresenterBase presenter)
        {
            this.presenter = presenter;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void FadeOut(Image fadeOutImage, float duration)
        {
            StartCoroutine(FadeOutCoroutine(fadeOutImage, duration));
        }

        public void FadeIn(Image fadeInImage, float duration)
        {
            StartCoroutine(FadeInCoroutine(fadeInImage, duration));
        }

        private IEnumerator FadeOutCoroutine(Image fadeOutImage, float fadeDuration)
        {
            Debug.Log("FadeOutCoroutine");

            Color color = fadeOutImage.color;
            float elpasedTime = 0f;

            while (elpasedTime < fadeDuration)
            {
                elpasedTime += Time.deltaTime;

                color = Color.Lerp(color, Color.black, elpasedTime / fadeDuration);
                fadeOutImage.color = color;

                yield return null;
            }

            fadeOutImage.gameObject.SetActive(false);
        }

        private IEnumerator FadeInCoroutine(Image fadeInImage, float fadeDuration)
        {
            Debug.Log("FadeInCoroutine");

            fadeInImage.gameObject.SetActive(true);
            fadeInImage.color = Color.black;
            Color color = fadeInImage.color;
            float elpasedTime = 0f;

            while (elpasedTime < fadeDuration)
            {
                elpasedTime += Time.deltaTime;
                color = Color.Lerp(color, Color.white, elpasedTime / fadeDuration);
                fadeInImage.color = color;

                yield return null;
            }
        }

        private IEnumerator FadeOutInCoroutine(Image fadeOutImage, Image fadeInImage, float fadeDuration)
        {
            yield return FadeOutCoroutine(fadeOutImage, fadeDuration);
            yield return FadeInCoroutine(fadeInImage, fadeDuration);
        }
    }
}
