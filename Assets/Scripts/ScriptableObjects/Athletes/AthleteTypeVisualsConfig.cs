using FormForge.Domain.Athletes;
using UnityEngine;

namespace FormForge.ScriptableObjects.Athletes
{
    [CreateAssetMenu(fileName = "AthleteTypeVisualsConfig", menuName = "Scriptable Objects/AthleteTypeVisualsConfig")]
    public class AthleteTypeVisualsConfig: ScriptableObject
    {
        public EAthleteType Type;
        public Sprite Icon;
        public Color ThemeColor;
    }
}