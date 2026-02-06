using System.Threading.Tasks;
using FormForge.Core;
using FormForge.Core.Services;
using FormForge.Infrastructure.SceneService;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.Messaging.Interfaces;
using FormForge.UI.FrontendStateMachine.Payloads;
using FormForge.UI.Screens.ViewModels;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class MainMenuState : FrontendState<MainMenuStatePayload>
    {
        public override async Task EnterAsync()
        {
            if (Payload != null && Payload.LoadScene)
            {
                ISceneService sceneService = ServiceLocator.GetService<ISceneService>();
                await sceneService.LoadSceneAsync(SceneIds.MainMenu);
                await sceneService.UnloadSceneAsync(SceneIds.Bootstrap);
            }

            ServiceLocator.GetService<IMessageService>().Send(new OpenScreenMessage(new MainMenuViewModel()));
        }

        public override Task ExitAsync()
        {
            ServiceLocator.GetService<IMessageService>().Send(new CloseScreenMessage(typeof(MainMenuViewModel)));
            return Task.CompletedTask;
        }
    }
}