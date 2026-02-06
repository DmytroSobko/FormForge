using System.Threading.Tasks;
using FormForge.Core;
using FormForge.Core.Services;
using FormForge.Infrastructure.SceneService;
using FormForge.UI.FrontendStateMachine.Payloads;

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
        }

        public override Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}