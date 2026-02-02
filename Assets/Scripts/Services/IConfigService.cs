using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Configs.Runtime;
using FormForge.Core.Domain;

namespace FormForge.Services
{
    public interface IConfigService
    {
        public IReadOnlyDictionary<string, AthleteType> AthleteTypes { get; }
        public IReadOnlyDictionary<string, Exercise> Exercises { get; }
        public IReadOnlyDictionary<EIntensityType, Intensity> Intensities { get; }
        public SimulationConfig Simulation { get; }
        
        Task LoadConfigsAsync();
    }
}