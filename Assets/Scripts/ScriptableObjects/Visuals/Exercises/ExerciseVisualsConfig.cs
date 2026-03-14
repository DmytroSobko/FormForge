using FormForge.Domain.Exercises;
using UnityEngine;

namespace FormForge.ScriptableObjects.Visuals.Exercises
{
    [CreateAssetMenu(fileName = "ExerciseVisualsConfig", menuName = "Scriptable Objects/Visuals/ExerciseVisualsConfig")]
    public class ExerciseVisualsConfig : ScriptableObject
    {        
        public EExerciseType Type;
    }
}