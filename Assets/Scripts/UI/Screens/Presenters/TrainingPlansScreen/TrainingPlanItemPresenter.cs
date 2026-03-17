using FormForge.Infrastructure.UI.Pagination;
using FormForge.UI.Screens.ViewModels.TrainingPlansScreen;
using FormForge.UI.Screens.Views.TrainingPlansScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.TrainingPlansScreen
{
    public class TrainingPlanItemPresenter: MonoBehaviour, IPaginatedItemPresenter<TrainingPlanItemViewModel>
    {
        [SerializeField] private TrainingPlanItemView m_TrainingPlanItemView;

        public void Initialize(TrainingPlanItemViewModel viewModel)
        {
            m_TrainingPlanItemView.Bind(viewModel);
        }
    }
}