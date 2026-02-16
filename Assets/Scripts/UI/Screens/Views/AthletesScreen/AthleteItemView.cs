using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.AthleteScreen
{
    public class AthleteItemView : MonoBehaviour
    {
        [SerializeField] private Image m_AthleteIcon;
        [SerializeField] private TextMeshProUGUI m_AthleteName;
        [SerializeField] private Button m_ItemButton;

        private Action m_OnItemClicked;

        private void Awake()
        {
          //  m_ItemButton.onClick.AddListener(OnItemPressed);
        }

        private void OnDestroy()
        {
          //  m_ItemButton.onClick.RemoveListener(OnItemPressed);
        }

        public void InitView(AthleteItemViewModel viewModel, Sprite athleteIcon, Action onItemClicked)
        {
            m_AthleteIcon.sprite = athleteIcon;
            m_AthleteName.text = viewModel.AthleteName;
            m_OnItemClicked = onItemClicked;
        }

        private void OnItemPressed()
        {
            m_OnItemClicked?.Invoke();
        }
    }
}