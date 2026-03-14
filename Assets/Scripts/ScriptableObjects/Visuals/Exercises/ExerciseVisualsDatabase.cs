using System.Collections.Generic;
using FormForge.Domain.Exercises;
using UnityEngine;

namespace FormForge.ScriptableObjects.Visuals.Exercises
{
    [CreateAssetMenu(fileName = "ExerciseVisualsDatabase", menuName = "Scriptable Objects/Visuals/ExerciseVisualsDatabase")]
    public class ExerciseVisualsDatabase : ScriptableObject
    {
        [SerializeField] private List<ExerciseVisualsConfig> m_Configs;

        private Dictionary<EExerciseType, ExerciseVisualsConfig> m_Lookup;

        public void Initialize()
        {
            m_Lookup = new Dictionary<EExerciseType, ExerciseVisualsConfig>();

            foreach (var cfg in m_Configs)
            {
                m_Lookup[cfg.Type] = cfg;
            }
        }

        public ExerciseVisualsConfig Get(EExerciseType type)
        {
            return m_Lookup[type];
        }
    }
}