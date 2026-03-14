using FormForge.Domain.Athletes;
using UnityEngine;

namespace FormForge.ScriptableObjects.Visuals.Athletes
{
    [CreateAssetMenu(fileName = "AthleteTypeVisualsConfig", menuName = "Scriptable Objects/Visuals/AthleteTypeVisualsConfig")]
    public class AthleteTypeVisualsConfig: ScriptableObject
    {
        public EAthleteType Type;
        public Sprite Icon;
        public Color ThemeColor;
    }
}