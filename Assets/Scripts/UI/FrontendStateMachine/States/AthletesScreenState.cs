using System.Threading.Tasks;
using FormForge.Core.Services;
using FormForge.Infrastructure.UI.LoadingOverlay.Messages;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.Messaging.Interfaces;
using FormForge.UI.Screens.ViewModels;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class AthletesScreenState : IFrontendState
    {
        public Task EnterAsync()
        {
            var messageService = ServiceLocator.GetService<IMessageService>();
            messageService.Send(new LoadingOverlayShowMessage());
            messageService.Send(new LoadingOverlaySetProgressMessage(0.5f));
            messageService.Send(new OpenScreenMessage(new AthletesScreenViewModel()));
            messageService.Send(new LoadingOverlaySetProgressMessage(1f));
            messageService.Send(new LoadingOverlayHideMessage());
            return Task.CompletedTask;
        }

        public Task ExitAsync()
        {
            ServiceLocator.GetService<IMessageService>().Send(new CloseScreenMessage(typeof(AthletesScreenViewModel)));
            return Task.CompletedTask;
        }
    }
}