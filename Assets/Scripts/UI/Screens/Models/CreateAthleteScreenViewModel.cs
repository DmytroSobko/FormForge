using System.Collections.Generic;
using FormForge.Infrastructure.UI.Screens.Models;
using FormForge.Runtime.Models.Athletes;

namespace FormForge.UI.Screens.Models
{
    public class CreateAthleteScreenViewModel : IScreenViewModel
    {
        public List<AthleteType> AthleteTypes { get; }

        public CreateAthleteScreenViewModel(List<AthleteType> athleteTypes)
        {
            AthleteTypes = athleteTypes;
        }
    }
}