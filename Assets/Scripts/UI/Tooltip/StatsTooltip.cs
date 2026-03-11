using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Overlays;
using FormForge.Services.InitializationService;
using FormForge.UI.Tooltip.Messages;
using FormForge.UI.Tooltip.Models;
using TMPro;
using UnityEngine;

namespace FormForge.UI.Tooltip
{
    public class StatsTooltip : FadableOverlayBase, 
        IMessageReceiver<StatsTooltipShowMessage>,
        IMessageReceiver<StatsTooltipHideMessage>
    {
        private const float k_ScreenPadding = 10f;
        
        [SerializeField] private Canvas m_Canvas;
        [SerializeField] private RectTransform m_Rect;
        [SerializeField] private TextMeshProUGUI m_TitleText;
        [SerializeField] private TextMeshProUGUI m_DescriptionText;
        [SerializeField] private Transform m_StatsContainer;
        
        private GameObject m_StatPrefab;

        private List<GameObject> m_Rows = new List<GameObject>();
        
        private IMessageService m_MessageService;
        
        private async void Awake()
        {
            HideImmediate();
            
            await WaitForInitialization();
            await LoadItemPrefab();
            
            m_MessageService = ServiceLocator.GetService<IMessageService>();

            AddListeners();
        }
        
        private async UniTask WaitForInitialization()
        {
            var initializationService = ServiceLocator.GetService<IInitializationService>();

            while (!initializationService.IsInitialized)
            {
                await UniTask.Yield();
            }
        }

        private async UniTask LoadItemPrefab()
        {
            var policy = new BasicAssetPolicy(AddressKeys.UI.Tooltips.StatRow);
            m_StatPrefab = await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<GameObject, UIContext>(policy);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            m_MessageService.Register<StatsTooltipShowMessage>(this);
            m_MessageService.Register<StatsTooltipHideMessage>(this);
        }
        
        private void RemoveListeners()
        {
            m_MessageService.Unregister<StatsTooltipShowMessage>(this);
            m_MessageService.Unregister<StatsTooltipHideMessage>(this);
        }
        
        private void Show(TooltipData data, Vector2 screenPos, bool immediate = false)
        {
            m_TitleText.text = data.Title;
            
            if (string.IsNullOrEmpty(data.Description))
            {
                m_DescriptionText.gameObject.SetActive(false);
            }
            else
            {
                m_DescriptionText.gameObject.SetActive(true);
                m_DescriptionText.text = data.Description;
            }
            
            ClearRows();

            foreach (var stat in data.Stats)
            {
                var row = Instantiate(m_StatPrefab, m_StatsContainer);
                row.GetComponent<TooltipStatRow>().Init(stat);
                m_Rows.Add(row);
            }

            PositionTooltip(screenPos);
            Show(immediate);
        }
        
        private void ClearRows()
        {
            foreach (var r in m_Rows)
            {
                Destroy(r.gameObject);
            }
            m_Rows.Clear();
        }

        private void PositionTooltip(Vector2 screenPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_CanvasGroup.transform as RectTransform,
                screenPos,
                m_Canvas.worldCamera,
                out var local);

            m_Rect.anchoredPosition = Clamp(local);
        }

        private Vector2 Clamp(Vector2 pos)
        {
            RectTransform canvasRect = m_Canvas.transform as RectTransform;
            Vector2 size = m_Rect.sizeDelta;

            if (canvasRect == null)
            {
                return pos;
            }
            
            float x = Mathf.Clamp(pos.x, 
                k_ScreenPadding, canvasRect.rect.width - size.x - k_ScreenPadding);
            float y = Mathf.Clamp(pos.y, 
                -canvasRect.rect.height + size.y + k_ScreenPadding, -k_ScreenPadding);

            return new Vector2(x, y);
        }
        
        public void HandleMessage(StatsTooltipShowMessage messageData = null)
        {
            Show(messageData.TooltipData, messageData.ScreenPos);
        }

        public void HandleMessage(StatsTooltipHideMessage messageData = null)
        {
            Hide();
        }
    }
}