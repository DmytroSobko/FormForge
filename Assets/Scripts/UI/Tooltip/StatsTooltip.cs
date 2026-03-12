using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Collections;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Misc;
using FormForge.Infrastructure.UI.Overlays;
using FormForge.Services.InitializationService;
using FormForge.UI.Tooltip.Messages;
using FormForge.UI.Tooltip.Models;
using TMPro;
using UnityEngine;

namespace FormForge.UI.Tooltip
{
    public class StatsTooltip : BaseTooltip, 
        IMessageReceiver<StatsTooltipShowMessage>,
        IMessageReceiver<StatsTooltipHideMessage>
    {
        private const int k_InitialRowPoolSize = 5;

        [SerializeField] private TextMeshProUGUI m_TitleText;
        [SerializeField] private TextMeshProUGUI m_DescriptionText;
        [SerializeField] private Transform m_StatsContainer;
        
        private GameObject m_StatRowPrefab;
        private Pool<PoolableObject> m_StatRowsPool;
        private readonly List<PoolableObject> m_AcquiredStatRows = new List<PoolableObject>();

        private IMessageService m_MessageService;
        
        private async void Awake()
        {
            HideImmediate();
            
            await WaitForInitialization();
            await LoadStatRowPrefab();
            
            m_StatRowsPool = new Pool<PoolableObject>(k_InitialRowPoolSize, m_StatRowPrefab);
            
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
            m_StatRowPrefab = await ServiceLocator.GetService<IAssetManagementService>().
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

            foreach (TooltipStat stat in data.Stats)
            {
                PoolableObject statRow = m_StatRowsPool.Acquire();
                statRow.transform.SetParent(m_StatsContainer); 
                statRow.GetComponent<TooltipStatRow>().Init(stat);
                m_AcquiredStatRows.Add(statRow);
            }

            PositionTooltip(screenPos);
            Show(immediate);
        }
        
        private void ClearRows()
        {
            foreach (PoolableObject row in m_AcquiredStatRows)
            {
                m_StatRowsPool.Recycle(row);
            }
            m_AcquiredStatRows.Clear();
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