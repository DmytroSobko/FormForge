using System;
using System.Collections;
using FormForge.Core.Services;
using FormForge.Infrastructure.UI.LoadingOverlay.Messages;
using FormForge.Messaging.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.Infrastructure.UI.LoadingOverlay
{
    public class LoadingOverlay : MonoBehaviour, 
        IMessageReceiver<LoadingOverlayShowMessage>,
        IMessageReceiver<LoadingOverlayHideMessage>,
        IMessageReceiver<LoadingOverlaySetProgressMessage>
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private Image m_ProgressFill;
        [SerializeField] private float m_FadeDuration;
        
        private IMessageService m_MessageService;
        
        private Coroutine m_FadeRoutine;
        
        private void Awake()
        {
            m_MessageService = ServiceLocator.GetService<IMessageService>();
            
            m_MessageService.Register<LoadingOverlayShowMessage>(this);
            m_MessageService.Register<LoadingOverlayHideMessage>(this);
            m_MessageService.Register<LoadingOverlaySetProgressMessage>(this);
        }

        private void OnDestroy()
        {
            m_MessageService.Unregister<LoadingOverlayShowMessage>(this);
            m_MessageService.Unregister<LoadingOverlayHideMessage>(this);
            m_MessageService.Unregister<LoadingOverlaySetProgressMessage>(this);
        }

        private void Show()
        {
            if (m_FadeRoutine != null)
            {
                StopCoroutine(m_FadeRoutine);
            }
            
            m_CanvasGroup.blocksRaycasts = true;
            m_CanvasGroup.interactable = true;
            gameObject.SetActive(true);
            
            m_FadeRoutine = StartCoroutine(Fade(0f, 1f));
        }

        private void Hide()
        {
            if (m_FadeRoutine != null)
            {
                StopCoroutine(m_FadeRoutine);
            }

            m_FadeRoutine = StartCoroutine(Fade(1f, 0f, () =>
            {
                m_CanvasGroup.blocksRaycasts = false;
                m_CanvasGroup.interactable = false;
                gameObject.SetActive(false);
            }));
        }

        private void SetProgress(float value)
        {
            value = Mathf.Clamp01(value);
            m_ProgressFill.fillAmount = value;
        }
        
        private IEnumerator Fade(float from, float to, Action onComplete = null)
        {
            float time = 0f;
            m_CanvasGroup.alpha = from;

            while (time < m_FadeDuration)
            {
                time += Time.deltaTime;
                float t = time / m_FadeDuration;
                m_CanvasGroup.alpha = Mathf.Lerp(from, to, t);
                yield return null;
            }

            m_CanvasGroup.alpha = to;
            onComplete?.Invoke();
        }

        public void HandleMessage(LoadingOverlayShowMessage messageData = null)
        {
            Show();
        }

        public void HandleMessage(LoadingOverlayHideMessage messageData = null)
        {
            Hide();
        }

        public void HandleMessage(LoadingOverlaySetProgressMessage messageData = null)
        {
            SetProgress(messageData.Progress);
        }
    }
}