using FormForge.Domain.Athletes;
using FormForge.Infrastructure.UI.Pagination;
using UnityEngine;

namespace FormForge.UI.Screens.ViewModels.AthletesScreen
{
    public class AthleteItemViewModel : IPaginatedItemViewModel
    {
        public EAthleteType AthleteType { get; }
        
        public Sprite AthleteIcon { get; }

        public string AthleteName { get; }

        public AthleteItemViewModel(EAthleteType athleteType, string athleteName, Sprite athleteIcon)
        {
            AthleteType = athleteType;
            AthleteName = athleteName;
            AthleteIcon = athleteIcon;
        }
    }
}