using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Infrastructure.StateMachine.States;

namespace FormForge.Infrastructure.StateMachine
{
    public class StateMachine<TState> where TState : class, IState
    {
        protected Dictionary<string, TState> m_States = new Dictionary<string, TState>();
        protected TState CurrentState { get; private set; }

        private bool m_IsTransitioning;
        
        protected async Task ChangeStateInternalAsync(TState newState)
        {
            if (m_IsTransitioning)
            {
                throw new InvalidOperationException("State transition already in progress.");
            }

            m_IsTransitioning = true;

            try
            {
                if (CurrentState != null)
                {
                    await CurrentState.ExitAsync();
                }

                CurrentState = newState;
                await CurrentState.EnterAsync();
            }
            finally
            {
                m_IsTransitioning = false;
            }
        }
    }
}