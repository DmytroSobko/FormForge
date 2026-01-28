using System;
using System.Collections.Generic;

namespace FormForge.Configs
{
    [Serializable]
    public class SimulationConfig
    {
        public int DaysInWeek;
        public float RestDayRecovery;
        public float MaxFatiguePenalty;
        public float HighFatigueThreshold;
        public Dictionary<string, float> IntensityMultipliers;
    }

    [Serializable]
    public class SimulationConfigEnvelope
    {
        public string Version;
        public SimulationConfig Simulation;
    }
}