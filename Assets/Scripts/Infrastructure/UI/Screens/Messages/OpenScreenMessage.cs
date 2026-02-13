using FormForge.Infrastructure.UI.Screens.Models;

namespace FormForge.Infrastructure.UI.Screens.Messages
{
    public class OpenScreenMessage
    {
        public IScreenViewModel ViewModel
        {
            get;
        }

        public OpenScreenMessage(IScreenViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}