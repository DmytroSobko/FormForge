using System.Collections.Generic;
using FormForge.Domain;
using FormForge.Runtime.Models.Athletes;
using FormForge.Runtime.Models.Exercises;
using FormForge.Runtime.Models.Intensities;
using FormForge.Runtime.Models.Simulation;

namespace FormForge.Services.ConfigsService
{
    public class ConfigsCacheModel
    {
        public IReadOnlyDictionary<EAthleteType, AthleteType> AthleteTypes;
        public IReadOnlyDictionary<EExerciseType, Exercise> Exercises;
        public IReadOnlyDictionary<EIntensityType, Intensity> Intensities;
        public SimulationConfig Simulation;
    }
}