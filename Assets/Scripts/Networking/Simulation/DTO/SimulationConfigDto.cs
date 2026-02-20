using System;

namespace FormForge.Networking.Simulation.DTO
{
    [Serializable]
    public class SimulationConfigDto
    {
        public float RestDayRecovery;
        public float MaxFatiguePenalty;
        public float HighFatigueThreshold;
    }
}