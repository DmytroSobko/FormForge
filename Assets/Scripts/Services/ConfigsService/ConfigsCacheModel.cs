using System.Collections.Generic;
using FormForge.Domain.Athletes;
using FormForge.Domain.Exercises;
using FormForge.Domain.Intensities;
using FormForge.Domain.Simulation;

namespace FormForge.Services.ConfigsService
{
    public class ConfigsCacheModel
    {
        public IReadOnlyDictionary<EAthleteType, AthleteTypeConfig> AthleteTypes;
        public IReadOnlyDictionary<EExerciseType, Exercise> Exercises;
        public IReadOnlyDictionary<EIntensityType, Intensity> Intensities;
        public SimulationConfig Simulation;
    }
}