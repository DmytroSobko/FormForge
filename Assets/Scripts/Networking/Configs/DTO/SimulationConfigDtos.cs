using System;
using System.Collections.Generic;

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
        public int DaysInWeek;
        public float RestDayRecovery;
        public float MaxFatiguePenalty;
        public float HighFatigueThreshold;
        public Dictionary<string, float> IntensityMultipliers;
    }
}