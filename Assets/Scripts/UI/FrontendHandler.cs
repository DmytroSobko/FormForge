using FormForge.Infrastructure.Misc;
using FormForge.Infrastructure.UI.Screens;
using UnityEngine;

namespace FormForge.UI
{
    public class FrontendHandler : PersistentSingleton<FrontendHandler>
    {
        [SerializeField] private Transform m_ScreenContainer;
        [SerializeField] private Transform m_PopupnContainer;

        private FrontendStateMachine.FrontendStateMachine m_FrontendStateMachine;
        private ScreenManager m_ScreenManager;
        
        private void Start()
        {
            m_FrontendStateMachine = new FrontendStateMachine.FrontendStateMachine();
            m_ScreenManager = new ScreenManager(m_ScreenContainer);
        }
    }
}