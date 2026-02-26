using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.Domain.Athletes;
using FormForge.Domain.Exercises;
using FormForge.Domain.Intensities;
using FormForge.Domain.Simulation;

namespace FormForge.Services.ConfigsService
{
    public interface IConfigsService
    {
        public IReadOnlyDictionary<EAthleteType, AthleteTypeConfig> AthleteTypes { get; }
        public IReadOnlyDictionary<EExerciseType, Exercise> Exercises { get; }
        public IReadOnlyDictionary<EIntensityType, Intensity> Intensities { get; }
        public SimulationConfig SimulationConfig { get; }
        
        UniTask LoadConfigsAsync();

        AthleteTypeConfig GetAthleteTypeConfig(EAthleteType type);
        Exercise GetExerciseConfig(EExerciseType type);
        Intensity GetIntensityConfig(EIntensityType type);
    }
}