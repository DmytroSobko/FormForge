namespace FormForge.UI.FrontendStateMachine.States
{
    public interface IPayloadReceivableState
    {
        void SetPayload(IFrontendStatePayload payload);
    }
}