using FormForge.Domain;
using FormForge.Infrastructure.UI.Pagination;

namespace FormForge.UI.Screens.Views.AthleteScreen
{
    public class AthleteItemViewModel : IPaginatedItemViewModel
    {
        public EAthleteType AthleteType { get; }
        public string AthleteName { get; }

        public AthleteItemViewModel(EAthleteType athleteType, string athleteName)
        {
            AthleteType = athleteType;
            AthleteName = athleteName;
        }
    }
}