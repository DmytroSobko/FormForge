using System;

namespace FormForge.Runtime.Models.Simulation
{
    [Serializable]
    public class SimulationConfig
    {
        public int DaysInWeek;
        public float RestDayRecovery;
        public float MaxFatiguePenalty;
        public float HighFatigueThreshold;
    }
}