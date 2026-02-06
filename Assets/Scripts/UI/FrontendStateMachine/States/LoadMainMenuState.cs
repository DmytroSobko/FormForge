using System.Threading.Tasks;
using FormForge.Core;
using FormForge.Core.Services;
using FormForge.Infrastructure.SceneService;
using FormForge.Messaging.Interfaces;
using FormForge.UI.FrontendStateMachine.Messages;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class LoadMainMenuState : IFrontendState
    {
        public async Task EnterAsync()
        {
            ISceneService sceneService = ServiceLocator.GetService<ISceneService>();
            
            await sceneService.LoadSceneAsync(SceneIds.MainMenu);
            
            var message = new SwitchFrontendStateMessage(FrontendStates.MainMenu);
            ServiceLocator.GetService<IMessageService>().Send(message);
        }

        public Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}