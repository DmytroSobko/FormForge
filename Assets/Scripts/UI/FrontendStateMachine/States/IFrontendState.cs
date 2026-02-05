using FormForge.Infrastructure.StateMachine.States;

namespace FormForge.UI.FrontendStateMachine.States
{
    public interface IFrontendState : IState
    {
        
    }

    public interface IFrontendState<out TPayload> : IFrontendState 
        where TPayload: IFrontendStatePayload
    {
        TPayload Payload { get; }
    }
    
    public interface IPayloadReceivableState
    {
        void SetPayload(IFrontendStatePayload payload);
    }
}