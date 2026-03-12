using System;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.UI.Screens.Views.BaseScreen
{
    public class FooterView : MonoBehaviour
    {
        [SerializeField] private Button m_HomeButton;
        
        public event Action HomeButtonClicked;
        
        private void Awake()
        {
            m_HomeButton.onClick.AddListener(OnHomeButtonClicked);
        }

        private void OnDestroy()
        {
            m_HomeButton.onClick.RemoveListener(OnHomeButtonClicked);
        }
        
        private void OnHomeButtonClicked()
        {
            HomeButtonClicked?.Invoke();
        }
    }
}