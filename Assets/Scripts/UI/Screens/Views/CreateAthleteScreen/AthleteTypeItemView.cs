using System;
using FormForge.UI.Screens.ViewModels.CreateAthleteScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.CreateAthleteScreen
{
    public class AthleteTypeItemView: MonoBehaviour
    {
        [SerializeField] private Button m_ItemButton;

        [SerializeField] private Image m_AthleteTypeIcon;
        [SerializeField] private Image m_BackgroundImage;
        [SerializeField] private TextMeshProUGUI m_AthleteTypeText;

        [SerializeField] private GameObject m_SelectedIndicator;
        [SerializeField] private Color m_UnselectedColor;
        [SerializeField] private Color m_SelectedColor;
        
        public event Action ItemClicked;

        private void Awake()
        {
            m_ItemButton.onClick.AddListener(OnItemClicked);
        }

        private void OnDestroy()
        { 
            m_ItemButton.onClick.RemoveListener(OnItemClicked);
        }

        public void Bind(AthleteTypeItemViewModel viewModel)
        {
            m_AthleteTypeIcon.sprite = viewModel.AthleteIcon;
            m_AthleteTypeText.text = viewModel.AthleteTypeConfig.DisplayName;
        }
        
        public void SetSelectedVisual(bool selected)
        {
            if (selected)
            {
                m_BackgroundImage.color = m_UnselectedColor;
                m_SelectedIndicator.SetActive(true);
            }
            else
            {
                m_BackgroundImage.color = m_SelectedColor;
                m_SelectedIndicator.SetActive(false);
            }
        }
        
        private void OnItemClicked()
        {
            ItemClicked?.Invoke();
        }
    }
}