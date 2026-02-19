using FormForge.Domain.Exercises;
using UnityEngine;

namespace FormForge.ScriptableObjects.Exercises
{
    [CreateAssetMenu(fileName = "ExerciseVisualsConfig", menuName = "Scriptable Objects/ExerciseVisualsConfig")]
    public class ExerciseVisualsConfig : ScriptableObject
    {        
        public EExerciseType Type;
        
    }
}