using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.Domain.Athletes;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.CacheService;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.HttpClientService;
using FormForge.Networking;
using FormForge.Networking.Athletes.DTO;
using FormForge.Networking.Athletes.Mapping;
using FormForge.Networking.Athletes.Requests;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.AthletesService
{
    public class AthletesService : IAthletesService
    {
        private const string k_AthletesCacheKey = "athletes";
        private static readonly TimeSpan s_CacheLifetime = TimeSpan.FromMinutes(5);

        public IReadOnlyDictionary<string, Athlete> Athletes { get; private set; }
        
        private ILogger m_Logger = new UnityLogger(nameof(AthletesService));

        private readonly ICacheService m_CacheService;
        private readonly IHttpClientService m_HttpClientService;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IAthletesService, AthletesService>(ServiceLifespan.LazySingleton);
        }
        
        public AthletesService()
        {
            m_HttpClientService = ServiceLocator.GetService<IHttpClientService>();
            m_CacheService = ServiceLocator.GetService<ICacheService>();
        }
        
        //TODO come back to this later
        public async UniTask<Athlete> CreateAthlete(EAthleteType athleteType, string athleteName)
        {
            m_Logger?.Log("Creating a new athlete...");

            CreateAthleteRequestDto requestDto = new CreateAthleteRequestDto(athleteType, athleteName);

            var dto = await m_HttpClientService.PostAsync<CreateAthleteRequestDto, AthleteDto>(
                APIEndpoints.Athletes.Base, requestDto);

            var athlete = AthleteMapper.Map(dto);

            m_CacheService.Update<IReadOnlyDictionary<string, Athlete>>(k_AthletesCacheKey,
                dict =>
                {
                    var newDict = new Dictionary<string, Athlete>(dict)
                    {
                        [athlete.Id] = athlete
                    };
                    return newDict;
                });

            return athlete;
        }

        public async UniTask<IReadOnlyList<Athlete>> GetAthletes()
        {
            m_Logger?.Log("Fetching created athletes...");

            var athletes = await m_CacheService.GetOrCreateAsync(
                k_AthletesCacheKey, GetAthletesServer, s_CacheLifetime);

            return new List<Athlete>(athletes.Values);
        }

        private async UniTask<IReadOnlyDictionary<string, Athlete>> GetAthletesServer()
        {
            m_Logger?.Log("Fetching athletes from server...");

            var response = await m_HttpClientService.GetAsync<AthletesEnvelopeDto>(
                APIEndpoints.Athletes.Base);

            var mapped = MapById(response.Athletes, AthleteMapper.Map);
            Athletes = mapped;

            return mapped;
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