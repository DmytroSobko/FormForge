using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.Domain;
using FormForge.Runtime.Models.Athletes;
using FormForge.Runtime.Models.Exercises;
using FormForge.Runtime.Models.Intensities;
using FormForge.Runtime.Models.Simulation;

namespace FormForge.Services.ConfigsService
{
    public interface IConfigsService
    {
        public IReadOnlyDictionary<EAthleteType, AthleteType> AthleteTypes { get; }
        public IReadOnlyDictionary<EExerciseType, Exercise> Exercises { get; }
        public IReadOnlyDictionary<EIntensityType, Intensity> Intensities { get; }
        public SimulationConfig SimulationConfig { get; }
        
        UniTask LoadConfigsAsync();

        AthleteType GetAthleteType(EAthleteType type);
        Exercise GetExercise(EExerciseType type);
        Intensity GetIntensity(EIntensityType type);
    }
}