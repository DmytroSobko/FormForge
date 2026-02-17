using System.Collections.Generic;
using FormForge.Infrastructure.UI.Screens.Models;
using FormForge.Runtime.Models.Athletes;

namespace FormForge.UI.Screens.Models
{
    public class CreateAthleteScreenViewModel : IScreenViewModel
    {
        public static string s_Address = "CreateAthleteScreen";

        public List<Athlete> Athletes { get; }

        public CreateAthleteScreenViewModel(List<Athlete> athletes)
        {
            Athletes = athletes;
        }
    }
}