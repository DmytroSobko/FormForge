using System;

namespace FormForge.Networking.Simulation.DTO
{
    [Serializable]
    public class SimulationConfigEnvelopeDto
    {
        public string Version;
        public SimulationConfigDto Simulation;
    }
}