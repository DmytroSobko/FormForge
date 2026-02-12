using System.Threading.Tasks;
using FormForge.Core.Services;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.Messaging.Interfaces;
using FormForge.UI.Screens.ViewModels;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class AthletesScreenState : IFrontendState
    {
        public async Task EnterAsync()
        {
            ServiceLocator.GetService<IMessageService>().Send(new OpenScreenMessage(new AthletesScreenViewModel()));
        }

        public Task ExitAsync()
        {
            ServiceLocator.GetService<IMessageService>().Send(new CloseScreenMessage(typeof(AthletesScreenViewModel)));
            return Task.CompletedTask;
        }
    }
}