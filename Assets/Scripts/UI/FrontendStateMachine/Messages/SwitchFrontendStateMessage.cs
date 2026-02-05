using FormForge.UI.FrontendStateMachine.States;

namespace FormForge.UI.FrontendStateMachine.Messages
{
    public class SwitchFrontendStateMessage
    {
        public string StateName
        {
            get;
        }
        
        public IFrontendStatePayload Payload 
        {
            get;
        }
        
        public SwitchFrontendStateMessage(string stateName, IFrontendStatePayload payload = null)
        {
            StateName = stateName;
            Payload = payload;
        }
    }
}