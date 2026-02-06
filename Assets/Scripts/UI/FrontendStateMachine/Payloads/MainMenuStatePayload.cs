using FormForge.UI.FrontendStateMachine.States;

namespace FormForge.UI.FrontendStateMachine.Payloads
{
    public class MainMenuStatePayload : IFrontendStatePayload
    {
        public bool LoadScene { get; }

        public MainMenuStatePayload(bool loadScene)
        {
            LoadScene = loadScene;
        }
    }
}