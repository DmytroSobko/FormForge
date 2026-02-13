using System;
using FormForge.Infrastructure.UI.Screens.Views;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views
{
    public class MainMenuScreenView : BaseScreenView
    {
        [SerializeField] private Button m_AthletesButton;

        private Action m_OnAthletesButton;
        
        private void OnEnable()
        {
            m_AthletesButton.onClick.AddListener(OnAthletesButtonPressed);
        }

        private void OnDisable()
        {
            m_AthletesButton.onClick.RemoveListener(OnAthletesButtonPressed);
        }

        public void InitView(Action onAthletesButton)
        {
            m_OnAthletesButton = onAthletesButton;
        }

        private void OnAthletesButtonPressed()
        {
            m_OnAthletesButton?.Invoke();
        }
    }
}