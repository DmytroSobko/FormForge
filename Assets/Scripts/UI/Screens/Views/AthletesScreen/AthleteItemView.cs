using System;
using FormForge.UI.Screens.ViewModels.AthletesScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.AthletesScreen
{
    public class AthleteItemView : MonoBehaviour
    {
        [SerializeField] private Image m_AthleteIcon;
        [SerializeField] private TextMeshProUGUI m_AthleteName;
        [SerializeField] private Button m_ItemButton;

        public event Action ItemClicked;

        private void Awake()
        {
            m_ItemButton.onClick.AddListener(OnItemClicked);
        }

        private void OnDestroy()
        { 
            m_ItemButton.onClick.RemoveListener(OnItemClicked);
        }

        public void Bind(AthleteItemViewModel viewModel)
        {
            m_AthleteName.text = viewModel.AthleteName;
        }

        private void OnItemClicked()
        {
            ItemClicked?.Invoke();
        }
    }
}