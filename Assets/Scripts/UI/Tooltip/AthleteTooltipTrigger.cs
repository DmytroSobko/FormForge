using System.Collections;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.UI.Tooltip.Messages;
using FormForge.UI.Tooltip.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace FormForge.UI.Tooltip
{
    public class AthleteTooltipTrigger : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField] private float m_HoverDelay = 0.5f;
        
        private TooltipData m_TooltipData;
        private Coroutine m_HoverRoutine;

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

            ServiceLocator.GetService<IMessageService>().Send(new StatsTooltipHideMessage());
        }

        private IEnumerator ShowTooltipDelayed()
        {
            yield return new WaitForSeconds(m_HoverDelay);

            var showMessage = new StatsTooltipShowMessage(m_TooltipData,Mouse.current.position.ReadValue());
            ServiceLocator.GetService<IMessageService>().Send(showMessage);
        }
    }
}