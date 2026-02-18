using System;

namespace FormForge.Networking.Configs.DTO
{
    [Serializable]
    public class SimulationConfigEnvelopeDto
    {
        public string Version;
        public SimulationConfigDto Simulation;
    }
    
    [Serializable]
    public class SimulationConfigDto
    {
        public float RestDayRecovery;
        public float MaxFatiguePenalty;
        public float HighFatigueThreshold;
    }
}