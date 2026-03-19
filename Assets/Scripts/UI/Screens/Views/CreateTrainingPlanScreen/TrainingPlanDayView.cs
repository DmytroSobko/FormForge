using System;
using FormForge.Infrastructure.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.CreateTrainingPlanScreen
{
    public class TrainingPlanDayView : MonoBehaviour
    {
        [SerializeField] private RectTransform m_ExerciseContainer;
        [SerializeField] private Button m_AddButton;

        private Pool<PoolableObject> m_ExerciseItemsPool;
        
        public event Action AddButtonClicked;
        
        private void Awake()
        {
            m_AddButton.onClick.AddListener(OnAddButtonClicked);
        }

        private void OnDestroy()
        {
            m_AddButton.onClick.RemoveListener(OnAddButtonClicked);
        }

        public void Init(Pool<PoolableObject> exerciseItemsPool)
        {
            m_ExerciseItemsPool = exerciseItemsPool;
        }

        private void OnAddButtonClicked()
        {
            AddButtonClicked?.Invoke();
        }
    }
}