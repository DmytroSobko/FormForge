using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Collections;
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
        private const int k_InitialRowPoolSize = 5;
        private const float k_ScreenPadding = 10f;
        private static readonly Vector2 s_Offset = new Vector2(20f, -20f);

        [SerializeField] private RectTransform m_Rect;
        [SerializeField] private TextMeshProUGUI m_TitleText;
        [SerializeField] private TextMeshProUGUI m_DescriptionText;
        [SerializeField] private Transform m_StatsContainer;
        
        private GameObject m_StatPrefab;
        private Pool<PoolableObject> m_RowPool;
        private List<PoolableObject> m_UsedRows = new List<PoolableObject>();

        private IMessageService m_MessageService;
        
        private async void Awake()
        {
            HideImmediate();
            
            await WaitForInitialization();
            await LoadStatRowPrefab();
            
            m_RowPool = new Pool<PoolableObject>(k_InitialRowPoolSize, m_StatPrefab);
            
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

        private async UniTask LoadStatRowPrefab()
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
                PoolableObject row = m_RowPool.Acquire();
                row.transform.SetParent(m_StatsContainer); 
                row.GetComponent<TooltipStatRow>().Init(stat);
                m_UsedRows.Add(row);
            }

            PositionTooltip(screenPos);
            Show(immediate);
        }
        
        private void ClearRows()
        {
            foreach (PoolableObject row in m_UsedRows)
            {
                m_RowPool.Recycle(row);
            }
            m_UsedRows.Clear();
        }
        
        private void PositionTooltip(Vector2 screenPos)
        {
            Vector2 pos = screenPos + s_Offset;

            var rect = m_Rect.rect;
            float width = rect.width;
            float height = rect.height;

            float x = Mathf.Clamp(pos.x, k_ScreenPadding, Screen.width - width - k_ScreenPadding);
            float y = Mathf.Clamp(pos.y, height + k_ScreenPadding, Screen.height - k_ScreenPadding);

            m_Rect.position = new Vector2(x, y);
        }

        public void HandleMessage(StatsTooltipShowMessage messageData = null)
        {
            if (messageData == null)
            {
                return;
            }
            Show(messageData.TooltipData, messageData.ScreenPos);
        }

        public void HandleMessage(StatsTooltipHideMessage messageData = null)
        {
            Hide();
        }
    }
}