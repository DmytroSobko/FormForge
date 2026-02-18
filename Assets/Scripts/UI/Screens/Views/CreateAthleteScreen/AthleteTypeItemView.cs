using System;
using FormForge.Infrastructure.UI.Selection;
using FormForge.Runtime.Models.Athletes;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.CreateAthleteScreen
{
    public class AthleteTypeItemView: MonoBehaviour, ISelectableItem<AthleteType>
    {
        [SerializeField] private Button m_Button;
        [SerializeField] private GameObject m_SelectedIndicator;

        public AthleteType Value { get; private set; }
        public bool IsSelected { get; private set; }

        public event Action<ISelectableItem<AthleteType>> OnSelected;

        public void Initialize(AthleteType athlete)
        {
            Value = athlete;
        }

        private void Awake()
        {
            m_Button.onClick.AddListener(() =>
            {
                OnSelected?.Invoke(this);
            });
        }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;
            m_SelectedIndicator.SetActive(selected);
        }
    }
}