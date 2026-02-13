using System.Threading.Tasks;
using FormForge.Core;
using FormForge.Core.Services;
using FormForge.Infrastructure.SceneService;
using FormForge.Infrastructure.UI.LoadingOverlay.Messages;
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
            var messageService = ServiceLocator.GetService<IMessageService>();
            
            if (Payload != null && Payload.LoadScene)
            {
                ISceneService sceneService = ServiceLocator.GetService<ISceneService>();
                await sceneService.LoadSceneAsync(SceneIds.MainMenu);
                await sceneService.UnloadSceneAsync(SceneIds.Bootstrap);
            }

            messageService.Send(new LoadingOverlaySetProgressMessage(0.9f));
            messageService.Send(new OpenScreenMessage(new MainMenuScreenViewModel()));
            messageService.Send(new LoadingOverlaySetProgressMessage(1f));
            messageService.Send(new LoadingOverlayHideMessage());
        }

        public override Task ExitAsync()
        {
            ServiceLocator.GetService<IMessageService>().Send(new CloseScreenMessage(typeof(MainMenuScreenViewModel)));
            return Task.CompletedTask;
        }
    }
}