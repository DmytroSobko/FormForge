using FormForge.Domain.Athletes;
using FormForge.Infrastructure.UI.Pagination;

namespace FormForge.UI.Screens.ViewModels.AthletesScreen
{
    public class AthleteItemViewModel : IPaginatedItemViewModel
    {
        public Athlete Athlete { get; }
        
        public AthleteItemViewModel(Athlete athlete)
        {
            Athlete = athlete;
        }
    }
}