using FormForge.Infrastructure.UI.Screens.Model;

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