using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Services.InitializationService;
using FormForge.UI.FrontendStateMachine;
using FormForge.UI.FrontendStateMachine.Messages;
using FormForge.UI.FrontendStateMachine.Payloads;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Core
{
    public class AppBootstrap : MonoBehaviour
    {
        private IInitializationService m_InitializationService;
        
        private ILogger m_Logger = new UnityLogger("AppBootstrap");

        private async void Start()
        {
            m_InitializationService = ServiceLocator.GetService<IInitializationService>();
            
            m_Logger?.Log("Initialization started");

            await m_InitializationService.Initialize();

            m_Logger?.Log("Initialization finished");
            
            var mainMenuMessage = new SwitchFrontendStateMessage(FrontendStates.MainMenu, 
                new MainMenuStatePayload(loadScene: true));
            ServiceLocator.GetService<IMessageService>().Send(mainMenuMessage);
        }
    }
}