using FormForge.Domain.Simulation;
using FormForge.Networking.Simulation.DTO;

namespace FormForge.Networking.Simulation.Mapping
{
    public static class SimulationConfigMapping
    {
        public static SimulationConfig Map(SimulationConfigDto dto)
        {
            return new SimulationConfig
            {
                RestDayRecovery = dto.RestDayRecovery,
                MaxFatiguePenalty = dto.MaxFatiguePenalty,
                HighFatigueThreshold = dto.HighFatigueThreshold,
            };
        }
    }
}