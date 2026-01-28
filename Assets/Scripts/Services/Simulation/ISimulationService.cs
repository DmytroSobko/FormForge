using System.Threading.Tasks;
using FormForge.Configs;

namespace FormForge.Services.Simulation
{
    public interface ISimulationService
    {
        SimulationConfig Config { get; }
        bool IsLoaded { get; }
        
        Task LoadSimulationConfig();
    }
}