using UnityEngine;

namespace FormForge.ScriptableObjects.Exercises
{
    [CreateAssetMenu(fileName = "ExerciseDefinitionSO", menuName = "Scriptable Objects/ExerciseDefinitionSO")]
    public class ExerciseDefinitionSO : ScriptableObject
    {
        [SerializeField] private string m_Id;
        [SerializeField] private Sprite m_Icon;

        public string Id => m_Id;
        public Sprite Icon => m_Icon;
    }
}