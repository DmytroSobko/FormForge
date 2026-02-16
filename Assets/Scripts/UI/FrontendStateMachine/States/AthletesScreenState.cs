using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Core.Services;
using FormForge.Infrastructure.UI.LoadingOverlay.Messages;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.Messaging.Interfaces;
using FormForge.Runtime.Models.Athletes;
using FormForge.Services.AthletesService;
using FormForge.UI.Screens.Models;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class AthletesScreenState : IFrontendState
    {
        public async Task EnterAsync()
        {
            var messageService = ServiceLocator.GetService<IMessageService>();
            messageService.Send(new LoadingOverlayShowMessage());
            messageService.Send(new LoadingOverlaySetProgressMessage(0.25f));

            IAthletesService athletesService = ServiceLocator.GetService<IAthletesService>();
            IReadOnlyList<Athlete> athletes = await athletesService.GetAthletes();
            
            messageService.Send(new LoadingOverlaySetProgressMessage(0.75f));
            
            messageService.Send(new OpenScreenMessage(new AthletesScreenViewModel(athletes)));
            
            messageService.Send(new LoadingOverlaySetProgressMessage(1f));
            messageService.Send(new LoadingOverlayHideMessage());
        }

        public Task ExitAsync()
        {
            ServiceLocator.GetService<IMessageService>().Send(new CloseScreenMessage(typeof(AthletesScreenViewModel)));
            return Task.CompletedTask;
        }
    }
}