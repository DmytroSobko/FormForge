using System.Collections.Generic;
using FormForge.Domain.Athletes;
using UnityEngine;

namespace FormForge.ScriptableObjects.Visuals.Athletes
{
    [CreateAssetMenu(fileName = "AthleteTypeVisualsDatabase", menuName = "Scriptable Objects/Visuals/AthleteTypeVisualsDatabase")]
    public class AthleteTypeVisualsDatabase : ScriptableObject
    {
        [SerializeField] private List<AthleteTypeVisualsConfig> m_Configs;

        private Dictionary<EAthleteType, AthleteTypeVisualsConfig> m_Lookup;

        public void Initialize()
        {
            m_Lookup = new Dictionary<EAthleteType, AthleteTypeVisualsConfig>();

            foreach (var cfg in m_Configs)
            {
                m_Lookup[cfg.Type] = cfg;
            }
        }

        public AthleteTypeVisualsConfig Get(EAthleteType type)
        {
            return m_Lookup[type];
        }
    }
}