using System;

namespace FormForge.Networking.Configs.DTO
{
    [Serializable]
    public class SimulationConfigEnvelopeDto
    {
        public string Version;
        public SimulationConfigDto Simulation;
    }
}