using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.StateMachine.States;

namespace FormForge.Infrastructure.StateMachine
{
    public class StateMachine<TState> where TState : class, IState
    {
        protected Dictionary<string, TState> m_States = new Dictionary<string, TState>();
        protected TState CurrentState { get; private set; }

        private bool m_IsTransitioning;
        
        protected virtual ILogger m_Logger => new UnityLogger(nameof(StateMachine));

        protected async Task ChangeStateInternalAsync(TState newState)
        {
            if (m_IsTransitioning)
            {
                throw new KeyNotFoundException($"State machine is transitioning.");
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