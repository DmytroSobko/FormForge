using FormForge.Infrastructure.UI.Pagination;
using FormForge.UI.Screens.ViewModels.AthletesScreen;
using FormForge.UI.Screens.Views.AthletesScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.AthletesScreen
{
    public class AthleteItemPresenter : MonoBehaviour, IPaginatedItemPresenter<AthleteItemViewModel>
    {
        [SerializeField] private AthleteItemView m_AthleteItemView;

        public void Initialize(AthleteItemViewModel viewModel)
        {
            AddListeners();
            m_AthleteItemView.Bind(viewModel);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            m_AthleteItemView.ItemClicked += OnItemClicked;
        }
        
        private void RemoveListeners()
        {
            m_AthleteItemView.ItemClicked -= OnItemClicked;
        }

        private void OnItemClicked()
        {
            // TODO show tooltip with profile details
        }
    }
}