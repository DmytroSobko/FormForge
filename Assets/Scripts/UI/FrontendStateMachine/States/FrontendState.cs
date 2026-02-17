using System;
using Cysharp.Threading.Tasks;
using FormForge.UI.FrontendStateMachine.Payloads;

namespace FormForge.UI.FrontendStateMachine.States
{
    public abstract class FrontendState<TPayload> :
        IFrontendState<TPayload>, IPayloadReceivableState
        where TPayload : class, IFrontendStatePayload
    {
        public TPayload Payload { get; private set; }

        void IPayloadReceivableState.SetPayload(IFrontendStatePayload payload)
        {
            if (payload == null)
            {
                Payload = null;
                return;
            }

            if (!(payload is TPayload typedPayload))
            {
                throw new InvalidOperationException(
                    $"Invalid payload type. Expected {typeof(TPayload).Name}, got {payload.GetType().Name}");
            }

            Payload = typedPayload;
        }

        public abstract UniTask EnterAsync();
        public abstract UniTask ExitAsync();
    }
}