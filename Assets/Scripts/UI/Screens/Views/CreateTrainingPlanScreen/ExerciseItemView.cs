using System;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.CreateTrainingPlanScreen
{
    public class ExerciseItemView : MonoBehaviour
    {
        [SerializeField] private Button m_RemoveButton;
        
        public event Action RemoveExerciseClicked;
        
        private void Awake()
        {
            m_RemoveButton.onClick.AddListener(OnRemoveExerciseClicked);
        }

        private void OnDestroy()
        { 
            m_RemoveButton.onClick.RemoveListener(OnRemoveExerciseClicked);
        }
        
        private void OnRemoveExerciseClicked()
        {
            RemoveExerciseClicked?.Invoke();
        }
    }
}