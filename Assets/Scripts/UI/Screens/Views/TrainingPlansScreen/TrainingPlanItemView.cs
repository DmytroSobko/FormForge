using FormForge.UI.Screens.ViewModels.TrainingPlansScreen;
using TMPro;
using UnityEngine;

namespace FormForge.UI.Screens.Views.TrainingPlansScreen
{
    public class TrainingPlanItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TrainingPlanName;
        
        public void Bind(TrainingPlanItemViewModel viewModel)
        {
            m_TrainingPlanName.text = viewModel.TrainingPlan.Name;
        }
    }
}