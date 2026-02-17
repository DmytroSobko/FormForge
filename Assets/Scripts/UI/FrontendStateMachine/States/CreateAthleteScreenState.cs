using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.LoadingOverlay.Messages;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.Runtime.Models.Athletes;
using FormForge.Services.ConfigsService;
using FormForge.UI.Screens.Models;
using FormForge.UI.Screens.Models.AthletesScreen;

namespace FormForge.UI.FrontendStateMachine.States
{
    public class CreateAthleteScreenState : IFrontendState
    {
        public async Task EnterAsync()
        {
            var messageService = ServiceLocator.GetService<IMessageService>();
            messageService.Send(new LoadingOverlayShowMessage());
            messageService.Send(new LoadingOverlaySetProgressMessage(0.25f));

            IConfigsService configsService = ServiceLocator.GetService<IConfigsService>();
            
            //IReadOnlyList<Athlete> athleteTypes = await configsService.GetAthletes();
            List<AthleteType> athleteTypes = null;
            messageService.Send(new LoadingOverlaySetProgressMessage(0.75f));

            messageService.Send(new OpenScreenMessage(new CreateAthleteScreenViewModel(athleteTypes)));

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