using System;

namespace FormForge.Networking.Simulation.DTO
{
    [Serializable]
    public class SimulationConfigResponse
    {
        public float RestDayRecovery;
        public float MaxFatiguePenalty;
        public float HighFatigueThreshold;
    }
}