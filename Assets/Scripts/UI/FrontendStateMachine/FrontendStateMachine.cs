using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.StateMachine;
using FormForge.UI.FrontendStateMachine.Messages;
using FormForge.UI.FrontendStateMachine.Payloads;
using FormForge.UI.FrontendStateMachine.States;

namespace FormForge.UI.FrontendStateMachine
{
    public class FrontendStateMachine : StateMachine<IFrontendState>,
        IMessageReceiver<SwitchFrontendStateMessage>
    {
        private readonly IMessageService m_MessageService;
        private string m_CurrentStateId;
        public string CurrentStateId => m_CurrentStateId;

        protected override ILogger m_Logger => new UnityLogger(nameof(FrontendStateMachine));
        
        public FrontendStateMachine()
        {
            m_States = new Dictionary<string, IFrontendState>
            {
                {FrontendStates.MainMenu, new MainMenuState()},
                {FrontendStates.AthletesScreen, new AthletesScreenState()},
            };
            
            m_MessageService = ServiceLocator.GetService<IMessageService>();
            m_MessageService.Register<SwitchFrontendStateMessage>(this);
        }
        
        ~FrontendStateMachine()
        {
            m_MessageService.Unregister<SwitchFrontendStateMessage>(this);
        }
        
        public async void HandleMessage(SwitchFrontendStateMessage messageData = null)
        {
            await ChangeStateAsync(messageData.StateName, messageData.Payload);
        }

        private async Task ChangeStateAsync(string newStateId, IFrontendStatePayload payload = null)
        {
            m_Logger?.Log($"Requested state change: '{m_CurrentStateId ?? "<none>"}' → '{newStateId}'");

            if (!m_States.TryGetValue(newStateId, out var newState))
            {
                m_Logger?.LogError($"State not registered: '{newStateId}'");
                throw new KeyNotFoundException($"State not registered: {newStateId}");
            }

            if (payload != null)
            {
                if (newState is IPayloadReceivableState receiver)
                {
                    m_Logger?.Log($"Assigning payload '{payload.GetType().Name}' to state '{newStateId}'");
                    receiver.SetPayload(payload);
                }
                else
                {
                    m_Logger?.LogError($"State '{newStateId}' does not accept payload '{payload.GetType().Name}'");
                    throw new InvalidOperationException($"State '{newStateId}' does not accept payload");
                }
            }

            var previousStateId = m_CurrentStateId;
            m_CurrentStateId = newStateId;

            m_Logger?.Log($"Transition start: '{previousStateId ?? "<none>"}' → '{newStateId}'");

            await ChangeStateInternalAsync(newState);

            m_Logger?.Log($"Transition completed. Active state: '{newStateId}'");
        }
    }
}