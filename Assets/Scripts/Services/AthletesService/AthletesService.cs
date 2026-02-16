using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Core.Services;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Networking;
using FormForge.Runtime.Models.Athletes;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.AthletesService
{
    public class AthletesService : IAthletesService
    {
        private readonly IHttpClientService m_HttpClient;

        private ILogger m_Logger = new UnityLogger(nameof(AthletesService));
        
        //TODO implement caching with updates and progress saving
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IAthletesService, AthletesService>(ServiceLifespan.LazySingleton);
        }
        
        public AthletesService()
        {
            m_HttpClient = ServiceLocator.GetService<IHttpClientService>();
        }
        public void CreateAthlete()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IReadOnlyList<Athlete>> GetAthletes()
        {
            return new List<Athlete>();
        }
    }
}