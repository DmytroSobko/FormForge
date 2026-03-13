using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FormForge.Domain.Athletes;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.CacheService;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.HttpClientService;
using FormForge.Networking;
using FormForge.Networking.Athletes;
using FormForge.Networking.Athletes.DTO;
using FormForge.Networking.Athletes.Mapping;
using FormForge.Networking.Athletes.Requests;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.AthletesService
{
    public class AthletesService : IAthletesService
    {
        private const int k_PageSize = 100;
        private const string k_AthletesCacheKey = "athletes";
        private static readonly TimeSpan s_CacheLifetime = TimeSpan.FromMinutes(5);
        
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
        
        public async UniTask<Athlete> CreateAthlete(EAthleteType athleteType, string athleteName)
        {
            m_Logger?.Log("Creating a new athlete...");

            CreateAthleteRequestDto requestDto = new CreateAthleteRequestDto(athleteType, athleteName);

            var dto = await m_HttpClientService.PostAsync<CreateAthleteRequestDto, AthleteDto>(
                AthleteEndpoints.Base, requestDto);

            var athlete = AthleteMapper.Map(dto);

            m_CacheService.Update<IReadOnlyDictionary<string, Athlete>>(k_AthletesCacheKey,
                dict =>
                {
                    dict ??= new Dictionary<string, Athlete>();
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

            var athletesDict = await m_CacheService.GetOrCreateAsync(
                k_AthletesCacheKey, GetAthletesServer, s_CacheLifetime);

            return athletesDict.Values as IReadOnlyList<Athlete> ?? athletesDict.Values.ToList();
        }


        private async UniTask<IReadOnlyDictionary<string, Athlete>> GetAthletesServer()
        {
            m_Logger?.Log("Fetching athletes from server...");

            int offset = 0;
            var allAthletes = new Dictionary<string, Athlete>();

            while (true)
            {
                string url = AthleteEndpoints.Paginated(k_PageSize, offset);

                var response = await m_HttpClientService.GetAsync<AthletesResponse>(url);

                var mapped = response.Athletes
                    .Select(AthleteMapper.Map)
                    .ToList();

                foreach (var athlete in mapped)
                {
                    allAthletes[athlete.Id] = athlete;
                }

                if (mapped.Count < k_PageSize)
                {
                    break;
                }

                offset += k_PageSize;
            }

            return allAthletes;
        }
    }
}