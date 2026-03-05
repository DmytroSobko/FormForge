using FormForge.Domain.Simulation;
using FormForge.Networking.Simulation.DTO;

namespace FormForge.Networking.Simulation.Mapping
{
    public static class SimulationConfigMapping
    {
        public static SimulationConfig Map(SimulationConfigResponse response)
        {
            return new SimulationConfig
            {
                RestDayRecovery = response.RestDayRecovery,
                MaxFatiguePenalty = response.MaxFatiguePenalty,
                HighFatigueThreshold = response.HighFatigueThreshold,
            };
        }
    }
}