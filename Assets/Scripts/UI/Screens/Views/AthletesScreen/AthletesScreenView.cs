using System;
using FormForge.Infrastructure.UI.Screens.Views;
using FormForge.UI.Screens.ViewModels.AthletesScreen;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.AthletesScreen
{
    public class AthletesScreenView : BaseScreenView
    {
        [SerializeField] private AthletesPaginationView m_PaginationView;
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

        public void Bind(AthletesScreenViewModel viewModel)
        {
            m_PaginationView.Initialize(viewModel.PaginatedDataProvider, viewModel.ItemPrefab);
        }

        private void OnCreateButtonClicked()
        {
            CreateButtonClicked?.Invoke();
        }
    }
}