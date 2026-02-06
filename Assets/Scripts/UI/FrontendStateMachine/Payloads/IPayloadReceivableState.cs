using FormForge.UI.FrontendStateMachine.States;

namespace FormForge.UI.FrontendStateMachine.Payloads
{
    public interface IPayloadReceivableState
    {
        void SetPayload(IFrontendStatePayload payload);
    }
}