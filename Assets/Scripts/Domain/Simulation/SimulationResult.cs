using System.Collections.Generic;

namespace FormForge.Domain.Simulation
{
    public class SimulationResult
    {
        public StatSnapshot Before;
        public StatSnapshot After;
        public float Efficiency;
        public List<string> Warnings;

        public SimulationResult(StatSnapshot before, StatSnapshot after, float efficiency, List<string> warnings)
        {
            Before = before;
            After = after;
            Efficiency = efficiency;
            Warnings = warnings;
        }
    }
}