using System.Threading.Tasks;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class MainMenuState : IFrontendState
    {
        public Task EnterAsync()
        {
            return Task.CompletedTask;
        }

        public Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}