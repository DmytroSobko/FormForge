using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.HttpClientService;
using UnityEngine;

namespace FormForge.Services.SimulationService
{
    public class SimulationService : ISimulationService
    {
        private readonly IHttpClientService m_HttpClient;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<ISimulationService, SimulationService>(ServiceLifespan.LazySingleton);
        }
        
        public SimulationService()
        {
            m_HttpClient = ServiceLocator.GetService<IHttpClientService>();
        }
    }
}