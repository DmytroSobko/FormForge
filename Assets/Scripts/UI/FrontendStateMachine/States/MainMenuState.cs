using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FormForge.Core;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.Services.SceneService;
using FormForge.Infrastructure.UI.LoadingOverlay.Messages;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.UI.FrontendStateMachine.Payloads;
using FormForge.UI.Screens.Models;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class MainMenuState : FrontendState<MainMenuStatePayload>
    {
        public override async UniTask EnterAsync()
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

        public override UniTask ExitAsync()
        {
            ServiceLocator.GetService<IMessageService>().Send(new CloseScreenMessage(typeof(MainMenuScreenViewModel)));
            return UniTask.CompletedTask;
        }
    }
}