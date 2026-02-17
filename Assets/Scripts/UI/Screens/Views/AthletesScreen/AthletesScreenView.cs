using System;
using FormForge.Infrastructure.UI.Screens.Views;
using FormForge.UI.Screens.Pagination.DataProviders;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.AthletesScreen
{
    public class AthletesScreenView : BaseScreenView
    {
        [SerializeField] private AthletesPaginationView m_PaginationView;
        [SerializeField] private Button m_CreateButton;

        private Action m_OnCreateClicked;

        private void Awake()
        {
            m_CreateButton.onClick.AddListener(OnCreateClicked);
        }

        private void OnDestroy()
        {
            m_CreateButton.onClick.RemoveListener(OnCreateClicked);
        }

        public void InitView(AthletesDataProvider dataProvider, GameObject itemPrefab,
            Action onCreateClicked, string noContentMessage = "")
        {
            m_OnCreateClicked = onCreateClicked;
            m_PaginationView.Initialize(dataProvider, itemPrefab, noContentMessage);
        }

        private void OnCreateClicked()
        {
            m_OnCreateClicked?.Invoke();
        }
    }
}