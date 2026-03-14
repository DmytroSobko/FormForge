using FormForge.Infrastructure.Misc;
using FormForge.Infrastructure.UI.Screens;
using FormForge.Infrastructure.UI.Toast;
using UnityEngine;

namespace FormForge.UI
{
    public class FrontendHandler : PersistentSingleton<FrontendHandler>
    {
        
        [SerializeField] private Transform m_ScreenCanvas;
        [SerializeField] private Transform m_PopupCanvas;
        [SerializeField] private ToastView m_ToastView;

        private FrontendStateMachine.FrontendStateMachine m_FrontendStateMachine;
        private ScreenManager m_ScreenManager;
        private ToastManager m_ToastManager;

        private void Start()
        {
            m_FrontendStateMachine = new FrontendStateMachine.FrontendStateMachine();
            m_ScreenManager = new ScreenManager(m_ScreenCanvas);
            m_ToastManager = new ToastManager(m_ToastView);
        }
        
        private void OnDestroy()
        {
            m_FrontendStateMachine?.Dispose();
            m_ToastManager?.Dispose();
            m_ScreenManager?.Dispose();
        }
    }
}