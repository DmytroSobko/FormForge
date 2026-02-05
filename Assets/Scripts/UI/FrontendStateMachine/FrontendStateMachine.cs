using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Core.Services;
using FormForge.Infrastructure.StateMachine;
using FormForge.Messaging.Interfaces;
using FormForge.UI.FrontendStateMachine.Messages;
using FormForge.UI.FrontendStateMachine.States;

namespace FormForge.UI.FrontendStateMachine
{
    public class FrontendStateMachine : StateMachine<IFrontendState>,
        IMessageReceiver<SwitchFrontendStateMessage>
    {
        private readonly IMessageService m_MessageService;
        private string m_CurrentStateId;
        public string CurrentStateId => m_CurrentStateId;

        public FrontendStateMachine()
        {
            m_States = new Dictionary<string, IFrontendState>
            {
                {FrontendStates.LoadMainMenu, new LoadMainMenuState()},
                {FrontendStates.MainMenu, new MainMenuState()},
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
            await ChangeStateAsync(messageData.StateName);
        }

        private async Task ChangeStateAsync(string newStateId, IFrontendStatePayload payload = null)
        {
            if (!m_States.TryGetValue(newStateId, out var newState))
            {
                throw new KeyNotFoundException($"State not registered: {newStateId}");
            }

            if (payload != null)
            {
                if (newState is IPayloadReceivableState receiver)
                {
                    receiver.SetPayload(payload);
                }
                else
                {
                    throw new InvalidOperationException($"State '{newStateId}' does not accept payload");
                }
            }

            m_CurrentStateId = newStateId;
            await ChangeStateInternalAsync(newState);
        }
    }
}