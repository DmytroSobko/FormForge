using System;
using System.Collections.Generic;
using FormForge.Domain;
using FormForge.Infrastructure.UI.Screens.Views;
using FormForge.Infrastructure.UI.Selection;
using FormForge.Runtime.Models.Athletes;
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
        private GameObject m_ItemPrefab;

        private readonly SingleSelectionController<AthleteType> m_SelectionController 
            = new SingleSelectionController<AthleteType>();
        
        private void Awake()
        {
            m_CreateButton.onClick.AddListener(OnCreateClicked);
        }

        private void OnDestroy()
        {
            m_CreateButton.onClick.RemoveListener(OnCreateClicked);
        }
        
        public void InitView(IReadOnlyDictionary<EAthleteType, AthleteType> athleteTypes,
            GameObject itemPrefab, Action onCreateClicked)
        {
            m_ItemPrefab = itemPrefab;
            m_OnCreateClicked = onCreateClicked;

            InitAthleteTypesScrollRect(athleteTypes);
        }

        private void InitAthleteTypesScrollRect(IReadOnlyDictionary<EAthleteType, AthleteType> athleteTypes)
        {
            foreach (var athlete in athleteTypes.Values)
            {
                var item = Instantiate(m_ItemPrefab, m_ScrollRectContent).GetComponent<AthleteTypeItemView>();
                item.Initialize(athlete);

                m_SelectionController.Register(item);
            }

            m_SelectionController.OnSelectionChanged += OnAthleteSelected;
        }
        
        private void OnAthleteSelected(AthleteType athleteType)
        {
            Debug.Log($"Selected: {athleteType.Type}");
        }
        
        private void OnCreateClicked()
        {
            m_OnCreateClicked?.Invoke();
        }
    }
}