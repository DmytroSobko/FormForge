using System;
using System.Collections;
using UnityEngine;

namespace FormForge.Infrastructure.UI.Overlays
{
    public abstract class FadableOverlayBase : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup m_CanvasGroup;
        [SerializeField] protected float m_FadeDuration = 0.2f;

        private Coroutine m_FadeRoutine;

        protected virtual void Show(bool immediate = false)
        {
            StopFade();

            m_CanvasGroup.blocksRaycasts = true;
            m_CanvasGroup.interactable = true;
            gameObject.SetActive(true);

            if (immediate || m_FadeDuration <= 0f)
            {
                m_CanvasGroup.alpha = 1f;
                return;
            }

            m_FadeRoutine = StartCoroutine(Fade(0f, 1f));
        }

        protected virtual void Hide(bool immediate = false)
        {
            StopFade();

            if (immediate || m_FadeDuration <= 0f)
            {
                m_CanvasGroup.alpha = 0f;
                m_CanvasGroup.blocksRaycasts = false;
                m_CanvasGroup.interactable = false;
                gameObject.SetActive(false);
                return;
            }

            m_FadeRoutine = StartCoroutine(Fade(1f, 0f, OnHideFadeComplete));
        }

        private void OnHideFadeComplete()
        {
            m_CanvasGroup.blocksRaycasts = false;
            m_CanvasGroup.interactable = false;
            gameObject.SetActive(false);
        }

        protected void ShowImmediate() => Show(true);
        protected void HideImmediate() => Hide(true);

        private void StopFade()
        {
            if (m_FadeRoutine == null)
            {
                return;
            }
            StopCoroutine(m_FadeRoutine);
            m_FadeRoutine = null;
        }

        private IEnumerator Fade(float from, float to, Action onComplete = null)
        {
            float time = 0f;
            m_CanvasGroup.alpha = from;

            while (time < m_FadeDuration)
            {
                time += Time.unscaledDeltaTime;
                float t = time / m_FadeDuration;
                m_CanvasGroup.alpha = Mathf.Lerp(from, to, t);
                yield return null;
            }

            m_CanvasGroup.alpha = to;
            onComplete?.Invoke();
        }
    }
}