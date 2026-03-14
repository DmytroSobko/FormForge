using System.Collections.Generic;
using FormForge.Infrastructure.UI.Toast;
using UnityEngine;

namespace FormForge.ScriptableObjects.Visuals.Toasts
{
    [CreateAssetMenu(fileName = "ToastVisualsDatabase", menuName = "Scriptable Objects/Visuals/ToastVisualsDatabase")]
    public class ToastVisualsDatabase : ScriptableObject
    {
        [SerializeField] private List<ToastVisualsConfig> m_Configs;

        private Dictionary<EToastType, ToastVisualsConfig> m_Lookup;

        public void Initialize()
        {
            m_Lookup = new Dictionary<EToastType, ToastVisualsConfig>();

            foreach (var cfg in m_Configs)
            {
                m_Lookup[cfg.Type] = cfg;
            }
        }

        public ToastVisualsConfig Get(EToastType type)
        {
            return m_Lookup[type];
        }
    }
}