using System;
using FormForge.Infrastructure.UI.Screens.Views;
using FormForge.UI.Screens.ViewModels.TrainingPlansScreen;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.TrainingPlansScreen
{
    public class TrainingPlansScreenView : BaseScreenView
    {
        [SerializeField] private TrainingPlansPaginatedView m_PaginatedView;
        [SerializeField] private Button m_CreateButton;

        public event Action CreateButtonClicked;

        private void Awake()
        {
            m_CreateButton.onClick.AddListener(OnCreateButtonClicked);
        }

        private void OnDestroy()
        {
            m_CreateButton.onClick.RemoveListener(OnCreateButtonClicked);
        }

        public void Bind(TrainingPlansScreenViewModel viewModel)
        {
            m_PaginatedView.Initialize(viewModel.PaginatedDataProvider, viewModel.ItemPrefab);
        }

        private void OnCreateButtonClicked()
        {
            CreateButtonClicked?.Invoke();
        }
    }
}