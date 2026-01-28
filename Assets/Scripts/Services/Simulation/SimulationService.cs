using System.Threading.Tasks;
using FormForge.Configs;
using FormForge.Core.Services;
using FormForge.Infrastructure.Networking;
using UnityEngine;

namespace FormForge.Services.Simulation
{
    public class SimulationService : ISimulationService
    {
        private const string ConfigUrl = "http://localhost:8080/api/config/simulation";

        private readonly IHttpClientService m_HttpClient;

        public SimulationConfig Config { get; private set; }
        public bool IsLoaded => Config != null;

        public SimulationService()
        {
            m_HttpClient = ServiceLocator.GetService<IHttpClientService>();
        }

        public async Task LoadSimulationConfig()
        {
            var envelope = await m_HttpClient.GetAsync<SimulationConfigEnvelope>(ConfigUrl);

            Config = envelope.Simulation;

            Debug.Log("Simulation config loaded.");
        }
    }
}