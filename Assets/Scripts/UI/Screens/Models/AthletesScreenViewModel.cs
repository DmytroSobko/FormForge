using System.Collections.Generic;
using FormForge.Infrastructure.UI.Screens.Models;
using FormForge.Runtime.Models.Athletes;

namespace FormForge.UI.Screens.Models
{
    public class AthletesScreenViewModel : IScreenViewModel
    {
        public static string s_Address = "AthletesScreen";

        public IReadOnlyList<Athlete> Athletes
        {
            get;
        }

        public AthletesScreenViewModel(IReadOnlyList<Athlete> athletes)
        {
            Athletes = athletes;
        }
    }
}