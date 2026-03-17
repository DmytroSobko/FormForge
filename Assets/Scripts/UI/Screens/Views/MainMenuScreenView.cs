using System;
using FormForge.Infrastructure.UI.Screens.Views;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views
{
    public class MainMenuScreenView : BaseScreenView
    {
        [SerializeField] private Button m_AthletesButton;
        [SerializeField] private Button m_TrainingPlansButton;

        private Action m_OnAthletesButton;
        private Action m_OnTrainingPlansButton;

        private void OnEnable()
        {
            m_AthletesButton.onClick.AddListener(OnAthletesButtonClicked);
            m_TrainingPlansButton.onClick.AddListener(OnTrainingPlansButtonClicked);
        }

        private void OnDisable()
        {
            m_AthletesButton.onClick.RemoveListener(OnTrainingPlansButtonClicked);
            m_TrainingPlansButton.onClick.RemoveListener(OnTrainingPlansButtonClicked);
        }

        public void InitView(Action onAthletesButton, Action onTrainingPlansButton)
        {
            m_OnAthletesButton = onAthletesButton;
            m_OnTrainingPlansButton = onTrainingPlansButton;
        }

        private void OnAthletesButtonClicked()
        {
            m_OnAthletesButton?.Invoke();
        }
        
        private void OnTrainingPlansButtonClicked()
        {
            m_OnTrainingPlansButton?.Invoke();
        }
    }
}