using FormForge.Domain.Athletes;
using FormForge.Infrastructure.UI.Pagination;
using UnityEngine;

namespace FormForge.UI.Screens.ViewModels.AthletesScreen
{
    public class AthleteItemViewModel : IPaginatedItemViewModel
    {
        public Athlete Athlete { get; }
        
        public Sprite AthleteIcon { get; }
        
        public AthleteItemViewModel(Athlete athlete, Sprite athleteIcon)
        {
            Athlete = athlete;
            AthleteIcon = athleteIcon;
        }
    }
}