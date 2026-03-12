using System;
using FormForge.Infrastructure.UI;
using FormForge.Infrastructure.UI.Selection;
using FormForge.UI.Screens.ViewModels.CreateAthleteScreen;
using FormForge.UI.Screens.Views.CreateAthleteScreen;
using FormForge.UI.Tooltip.Components;
using FormForge.UI.Tooltip.Factories;
using FormForge.UI.Tooltip.Models;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.CreateAthleteScreen
{
    public class AthleteTypeItemPresenter : MonoBehaviour, IPresenter, ISelectableItem<AthleteTypeItemViewModel>
    { 
        [SerializeField] private AthleteTypeItemView m_View;
        [SerializeField] private AthleteTooltipTrigger m_TooltipTrigger;

        public AthleteTypeItemViewModel ViewModel
        {
            get; private set;
        }
        
        public event Action<ISelectableItem<AthleteTypeItemViewModel>> ItemSelected;
        
        public void Initialize(AthleteTypeItemViewModel viewModel)
        {
            ViewModel = viewModel;
            m_View.ItemClicked += OnItemClicked;
            
            m_View.Bind(ViewModel);
            
            TooltipData tooltipData = AthleteTooltipFactory.Create(viewModel.AthleteTypeConfig);
            m_TooltipTrigger.Bind(tooltipData);
        }

        private void OnItemClicked()
        {
            ItemSelected?.Invoke(this);
        }

        public void SetSelected(bool selected)
        {
            m_View.SetSelectedVisual(selected);
        }
    }
}