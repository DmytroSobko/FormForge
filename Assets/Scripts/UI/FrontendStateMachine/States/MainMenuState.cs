using System.Threading.Tasks;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class MainMenuState : FrontendState<MainMenuPayload>
    {
        public override Task EnterAsync()
        {
            throw new System.NotImplementedException();
        }

        public override Task ExitAsync()
        {
            throw new System.NotImplementedException();
        }
    }

    public class MainMenuPayload : IFrontendStatePayload
    {
        public string Message { get; }
    }
}