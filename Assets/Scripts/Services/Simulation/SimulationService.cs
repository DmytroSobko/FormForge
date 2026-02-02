using System.Threading.Tasks;
using FormForge.Core.Services;
using FormForge.Infrastructure.Networking;
using UnityEngine;

namespace FormForge.Services.Simulation
{
    public class SimulationService : ISimulationService
    {
        private const string ConfigUrl = "http://localhost:8080/api/config/simulation";

        private readonly IHttpClientService m_HttpClient;

        public SimulationService()
        {
            m_HttpClient = ServiceLocator.GetService<IHttpClientService>();
        }
    }
}