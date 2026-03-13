using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.Infrastructure.UI.Toast
{
    public class ToastView : MonoBehaviour
    {
        private const float k_FadeDuration = 0.2f;
        private const float k_SlideDistance = 40f;
        
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private RectTransform m_ContentRect;
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private Image m_Icon;

        private Coroutine m_Routine;
        
        private void OnDisable()
        {
            if (m_Routine == null)
            {
                return;
            }
            StopCoroutine(m_Routine);
            m_Routine = null;
        }

        public void Show(ToastShowMessage message, Action onComplete)
        {
            if (m_Routine != null)
            {
                StopCoroutine(m_Routine);
            }

            m_Text.text = message.Toast;
            SetIcon(message.Type);

            gameObject.SetActive(true);

            m_Routine = StartCoroutine(ShowRoutine(message.Duration, onComplete));
        }

        private IEnumerator ShowRoutine(float duration, Action onComplete)
        {
            yield return AnimateIn();
            yield return new WaitForSecondsRealtime(duration);
            yield return AnimateOut();

            gameObject.SetActive(false);

            onComplete?.Invoke();
        }

        private IEnumerator AnimateIn()
        {
            float time = 0f;

            m_CanvasGroup.alpha = 0f;
            m_ContentRect.anchoredPosition = new Vector2(0, k_SlideDistance);

            while (time < k_FadeDuration)
            {
                time += Time.unscaledDeltaTime;
                float t = time / k_FadeDuration;
                m_CanvasGroup.alpha = t;
                
                var from = new Vector2(0, k_SlideDistance);
                var to = Vector2.zero;
                m_ContentRect.anchoredPosition = Vector2.Lerp(from, to, t);

                yield return null;
            }

            m_CanvasGroup.alpha = 1f;
            m_ContentRect.anchoredPosition = Vector2.zero;
        }

        private IEnumerator AnimateOut()
        {
            float time = 0f;

            while (time < k_FadeDuration)
            {
                time += Time.unscaledDeltaTime;
                float t = time / k_FadeDuration;
                m_CanvasGroup.alpha = 1f - t;
                yield return null;
            }

            m_CanvasGroup.alpha = 0f;
        }

        private void SetIcon(ToastType type) 
        {
            switch (type)
            {
                case ToastType.Success:
                    m_Icon.color = new Color(0.2f, 0.6f, 0.2f);
                    break;

                case ToastType.Warning:
                    m_Icon.color = new Color(0.8f, 0.6f, 0.2f);
                    break;

                case ToastType.Error:
                    m_Icon.color = new Color(0.8f, 0.2f, 0.2f);
                    break;

                default:
                    m_Icon.color = new Color(0.2f, 0.2f, 0.2f);
                    break;
            }
        }
    }
}