using FormForge.Infrastructure.UI.Pagination;
using FormForge.UI.Screens.ViewModels.AthletesScreen;
using FormForge.UI.Screens.Views.AthletesScreen;
using FormForge.UI.Tooltip.Components;
using FormForge.UI.Tooltip.Factories;
using FormForge.UI.Tooltip.Models;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.AthletesScreen
{
    public class AthleteItemPresenter : MonoBehaviour, IPaginatedItemPresenter<AthleteItemViewModel>
    {
        [SerializeField] private AthleteItemView m_AthleteItemView;
        [SerializeField] private AthleteTooltipTrigger m_TooltipTrigger;

        public void Initialize(AthleteItemViewModel viewModel)
        {
            m_AthleteItemView.Bind(viewModel);
            
            TooltipData tooltipData = AthleteTooltipFactory.Create(viewModel.Athlete);
            m_TooltipTrigger.Bind(tooltipData);
        }
    }
}