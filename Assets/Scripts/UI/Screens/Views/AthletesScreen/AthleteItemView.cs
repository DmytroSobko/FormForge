using FormForge.Infrastructure.Services;
using FormForge.Services.VisualsService;
using FormForge.UI.Screens.ViewModels.AthletesScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.AthletesScreen
{
    public class AthleteItemView : MonoBehaviour
    {
        [SerializeField] private Image m_AthleteTypeIcon;
        [SerializeField] private TextMeshProUGUI m_AthleteName;
        
        public void Bind(AthleteItemViewModel viewModel)
        {
            var visualsService = ServiceLocator.GetService<IVisualsService>();
            m_AthleteTypeIcon.sprite = visualsService.GetAthleteTypeVisuals(viewModel.Athlete.Type).Icon;
            m_AthleteName.text = viewModel.Athlete.Name;
        }
    }
}