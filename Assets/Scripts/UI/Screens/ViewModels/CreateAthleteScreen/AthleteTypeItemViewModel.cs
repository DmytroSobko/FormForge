using FormForge.Domain.Athletes;
using FormForge.Infrastructure.UI.Screens.ViewModels;
using UnityEngine;

namespace FormForge.UI.Screens.ViewModels.CreateAthleteScreen
{
    public class AthleteTypeItemViewModel : IItemViewModel
    {
        public AthleteTypeConfig AthleteTypeConfig { get; }
        public Sprite AthleteIcon { get; }
        
        public AthleteTypeItemViewModel(AthleteTypeConfig athleteTypeConfig, Sprite athleteIcon)
        {
            AthleteTypeConfig = athleteTypeConfig;
            AthleteIcon = athleteIcon;
        }
    }
}