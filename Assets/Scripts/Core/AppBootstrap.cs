using System.Threading.Tasks;
using FormForge.Core.Services;
using FormForge.Infrastructure.Logging;
using FormForge.Messaging.Interfaces;
using FormForge.Services.Initialization;
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

        private async void Awake()
        {
            m_InitializationService = ServiceLocator.GetService<IInitializationService>();
            
            await InitializeAsync();
            
            var message = new SwitchFrontendStateMessage(FrontendStates.MainMenu, 
                new MainMenuStatePayload(loadScene: true));
            ServiceLocator.GetService<IMessageService>().Send(message);
        }
        
        private async Task InitializeAsync()
        {
            m_Logger?.Log("Initialization started");

            await m_InitializationService.Initialize();

            m_Logger?.Log("Initialization finished");
        }
    }
}