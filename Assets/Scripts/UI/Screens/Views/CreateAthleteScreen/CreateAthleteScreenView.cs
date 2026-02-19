using System;
using FormForge.Infrastructure.UI.Screens.Views;
using FormForge.UI.Screens.ViewModels.CreateAthleteScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.CreateAthleteScreen
{
    public class CreateAthleteScreenView: BaseScreenView
    {
        [SerializeField] private ScrollRect m_AthleteTypesScrollRect;
        [SerializeField] private RectTransform m_ScrollRectContent;
        [SerializeField] private TMP_InputField m_AthleteName;
        [SerializeField] private Button m_CreateButton;

        private Action m_OnCreateClicked;
        private GameObject m_AthleteTypeItemPrefab;

        public event Action<string> OnCreateClicked;
        
        private void Awake()
        {
            m_CreateButton.onClick.AddListener(OnCreateButtonClicked);
        }

        private void OnDestroy()
        {
            m_CreateButton.onClick.RemoveListener(OnCreateButtonClicked);
        }
        
        public void Bind(CreateAthleteScreenViewModel viewModel)
        {
            m_AthleteTypeItemPrefab = viewModel.ItemPrefab;
        }

        public GameObject CreateAthleteTypeItem()
        {
            return Instantiate(m_AthleteTypeItemPrefab, m_ScrollRectContent);
        }

        private void OnCreateButtonClicked()
        {
            OnCreateClicked?.Invoke(m_AthleteName.text);
        }
    }
}