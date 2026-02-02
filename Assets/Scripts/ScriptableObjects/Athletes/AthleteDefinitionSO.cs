using UnityEngine;

namespace FormForge.ScriptableObjects.Athletes
{
    [CreateAssetMenu(fileName = "AthleteDefinitionSO", menuName = "Scriptable Objects/AthleteDefinitionSO")]
    public class AthleteDefinitionSO : ScriptableObject
    {
        [SerializeField] private string m_Id;
        [SerializeField] private Sprite m_Icon;

        public string Id => m_Id;
        public Sprite Icon => m_Icon;
    }
}