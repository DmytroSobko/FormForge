using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Domain;
using FormForge.Runtime.Models.Athletes;
using FormForge.Runtime.Models.Exercises;
using FormForge.Runtime.Models.Intensities;
using FormForge.Runtime.Models.Simulation;

namespace FormForge.Services.ConfigsService
{
    public interface IConfigsService
    {
        public IReadOnlyDictionary<string, AthleteType> AthleteTypes { get; }
        public IReadOnlyDictionary<string, Exercise> Exercises { get; }
        public IReadOnlyDictionary<EIntensityType, Intensity> Intensities { get; }
        public SimulationConfig Simulation { get; }
        
        Task LoadConfigsAsync();
    }
}