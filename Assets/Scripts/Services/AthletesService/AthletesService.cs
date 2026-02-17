using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Core.Networking;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.HttpClientService;
using FormForge.Networking.Athletes.DTO;
using FormForge.Networking.Athletes.Mapping;
using FormForge.Runtime.Models.Athletes;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.AthletesService
{
    public class AthletesService : IAthletesService
    {
        private readonly IHttpClientService m_HttpClient;

        private ILogger m_Logger = new UnityLogger(nameof(AthletesService));

        public IReadOnlyDictionary<string, Athlete> Athletes { get; private set; }
        
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
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Athlete>> GetAthletes()
        {
            m_Logger?.Log("Fetching created athletes...");
            var athleteDto = await m_HttpClient.GetAsync<AthletesEnvelopeDto>(
                APIEndpoints.Athletes.Base);
            
            Athletes = MapById(athleteDto.Athletes, AthletesMapper.Map);
            return new List<Athlete>();
        }
        
        private static Dictionary<string, T> MapById<TDto, T>(List<TDto> list, Func<TDto, T> map) where T : class
        {
            var dict = new Dictionary<string, T>();
            foreach (var item in list)
            {
                var mapped = map(item);
                dict[(string)typeof(T).GetField("Id").GetValue(mapped)] = mapped;
            }
            return dict;
        }
    }
}