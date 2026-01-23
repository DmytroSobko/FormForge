using System;
using System.Collections.Generic;

namespace FormForge.Simulation
{
    [Serializable]
    public class StatSnapshot
    {
        public float Strength;
        public float Endurance;
        public float Mobility;
        public float Fatigue;

        public StatSnapshot(float strength, float endurance, float mobility, float fatigue)
        {
            Strength = strength;
            Endurance = endurance;
            Mobility = mobility;
            Fatigue = fatigue;
        }
    }

    [Serializable]
    public class SimulationResult
    { 
        public int Week;
        public StatSnapshot Before;
        public StatSnapshot After;
        public float Efficiency;
        public List<string> Warnings;

        public SimulationResult(int week, StatSnapshot before, StatSnapshot after, float efficiency,
            List<string> warnings)
        {
            Week = week;
            Before = before;
            After = after;
            Efficiency = efficiency;
            Warnings = warnings;
        }
    }
}