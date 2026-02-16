using System;
using System.Collections;
using UnityEngine;

namespace FormForge.Infrastructure.UI.Pagination
{
    public class PageAnimator : MonoBehaviour
    {
        [SerializeField] private RectTransform m_ContentRoot;
        [SerializeField] private float m_Duration = 0.25f;

        private Coroutine m_Animation;

        public void Animate(int direction, Action onSwap)
        {
            if (m_Animation != null)
            {
                StopCoroutine(m_Animation);
            }

            m_Animation = StartCoroutine(Slide(direction, onSwap));
        }

        private IEnumerator Slide(int direction, Action onSwap)
        {
            float width = m_ContentRoot.rect.width;
            float time = 0f;

            Vector2 start = Vector2.zero;
            Vector2 end = new Vector2(-direction * width, 0);

            while (time < m_Duration)
            {
                time += Time.deltaTime;
                float t = time / m_Duration;
                m_ContentRoot.anchoredPosition = Vector2.Lerp(start, end, t);
                yield return null;
            }

            onSwap?.Invoke();

            m_ContentRoot.anchoredPosition = new Vector2(direction * width, 0);

            time = 0f;

            while (time < m_Duration)
            {
                time += Time.deltaTime;
                float t = time / m_Duration;
                m_ContentRoot.anchoredPosition = Vector2.Lerp(
                    new Vector2(direction * width, 0),
                    Vector2.zero,
                    t
                );
                yield return null;
            }

            m_ContentRoot.anchoredPosition = Vector2.zero;
        }
    }
}