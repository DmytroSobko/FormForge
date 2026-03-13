using System.Collections;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.UI.Tooltip.Messages;
using FormForge.UI.Tooltip.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace FormForge.UI.Tooltip.Components
{
    public class AthleteTooltipTrigger : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float m_HoverDelay = 0.5f;
        
        private TooltipData m_TooltipData;
        private Coroutine m_HoverRoutine;

        private bool m_IsTooltipActivated;

        public void Bind(TooltipData data)
        {
            m_TooltipData = data;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            m_HoverRoutine = StartCoroutine(ShowTooltipDelayed());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (m_HoverRoutine != null)
            {
                StopCoroutine(m_HoverRoutine);
                m_HoverRoutine = null;
            }

            if (!m_IsTooltipActivated)
            {
                return;
            }
            ServiceLocator.GetService<IMessageService>().Send(new AthleteStatsTooltipHideMessage());
            m_IsTooltipActivated = false;
        }

        private IEnumerator ShowTooltipDelayed()
        {
            yield return new WaitForSeconds(m_HoverDelay);

            var showMessage = new AthleteStatsTooltipShowMessage(
                m_TooltipData, Mouse.current.position.ReadValue());
            ServiceLocator.GetService<IMessageService>().Send(showMessage);
            m_IsTooltipActivated = true;
        }
    }
}