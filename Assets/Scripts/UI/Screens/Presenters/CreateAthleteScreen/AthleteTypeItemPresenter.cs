using System;
using FormForge.Infrastructure.UI;
using FormForge.Infrastructure.UI.Selection;
using FormForge.UI.Screens.ViewModels.CreateAthleteScreen;
using FormForge.UI.Screens.Views.CreateAthleteScreen;
using UnityEngine;

namespace FormForge.UI.Screens.Presenters.CreateAthleteScreen
{
    public class AthleteTypeItemPresenter : MonoBehaviour, IPresenter, ISelectableItem<AthleteTypeItemViewModel>
    { 
        [SerializeField] private AthleteTypeItemView m_View;

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