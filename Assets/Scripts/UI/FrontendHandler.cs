using FormForge.Infrastructure.Misc;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.UI.Screens;
using FormForge.Infrastructure.UI.Toast;
using FormForge.Services.InitializationService;
using UnityEngine;

namespace FormForge.UI
{
    public class FrontendHandler : PersistentSingleton<FrontendHandler>
    {
        
        [SerializeField] private Transform m_ScreenCanvas;
        [SerializeField] private Transform m_PopupCanvas;
        [SerializeField] private Transform m_ToastContainer;

        private FrontendStateMachine.FrontendStateMachine m_FrontendStateMachine;
        private ScreenManager m_ScreenManager;
        private ToastManager m_ToastManager;

        private async void Start()
        {
            m_FrontendStateMachine = new FrontendStateMachine.FrontendStateMachine();
            m_ScreenManager = new ScreenManager(m_ScreenCanvas);
            
            await ServiceLocator
                .GetService<IInitializationService>()
                .WaitUntilInitialized();

            m_ToastManager = new ToastManager();
            m_ToastManager.Init(m_ToastContainer);
        }
    }
}