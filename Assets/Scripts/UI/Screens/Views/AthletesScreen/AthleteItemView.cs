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
            m_AthleteTypeIcon.sprite = viewModel.AthleteIcon;
            m_AthleteName.text = viewModel.Athlete.Name;
        }
    }
}