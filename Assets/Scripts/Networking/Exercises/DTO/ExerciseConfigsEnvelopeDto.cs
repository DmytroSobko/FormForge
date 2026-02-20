using System.Collections.Generic;

namespace FormForge.Networking.Exercises.DTO
{
    [System.Serializable]
    public class ExerciseConfigsEnvelopeDto
    {
        public string Version;
        public List<ExerciseConfigDto> Exercises;
    }
}