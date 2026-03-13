using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FormForge.Domain.Athletes;
using FormForge.Domain.Exercises;
using FormForge.Domain.Intensities;
using FormForge.Domain.Simulation;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.CacheService;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.HttpClientService;
using FormForge.Networking;
using FormForge.Networking.AthleteTypeConfigs.DTO;
using FormForge.Networking.AthleteTypeConfigs.Mapping;
using FormForge.Networking.Exercises.DTO;
using FormForge.Networking.Exercises.Mapping;
using FormForge.Networking.Intensities.DTO;
using FormForge.Networking.Intensities.Mapping;
using FormForge.Networking.Simulation.DTO;
using FormForge.Networking.Simulation.Mapping;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.ConfigsService
{
    public class ConfigsService : IConfigsService
    {
        private const string k_ConfigsCacheKey = "configs_all";
        private static readonly TimeSpan s_CacheLifetime = TimeSpan.FromHours(1);

        public IReadOnlyDictionary<EAthleteType, AthleteTypeConfig> AthleteTypes { get; private set; }
        public IReadOnlyDictionary<EExerciseType, Exercise> Exercises { get; private set; }
        public IReadOnlyDictionary<EIntensityType, Intensity> Intensities { get; private set; }
        public SimulationConfig SimulationConfig { get; private set; }

        private ILogger m_Logger = new UnityLogger(nameof(ConfigsService));

        private readonly IHttpClientService m_HttpClientService;
        private readonly ICacheService m_CacheService;
        private ConfigsCacheModel m_Configs;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IConfigsService, ConfigsService>(ServiceLifespan.LazySingleton);
        }

        public ConfigsService()
        {
            m_HttpClientService = ServiceLocator.GetService<IHttpClientService>();
            m_CacheService = ServiceLocator.GetService<ICacheService>();
        }

        public async UniTask LoadConfigsAsync()
        {
            m_Logger?.Log("Starting config load");
            try
            {
                m_Configs = await m_CacheService.GetOrCreateAsync(k_ConfigsCacheKey, 
                    FetchConfigsFromServerAsync, s_CacheLifetime);

                AthleteTypes = m_Configs.AthleteTypes;
                Exercises = m_Configs.Exercises;
                Intensities = m_Configs.Intensities;
                SimulationConfig = m_Configs.Simulation;

                m_Logger?.Log("Configs loaded successfully");
            }
            catch (Exception ex)
            {
                m_Logger?.LogError("Failed to load configs");
                m_Logger?.LogException(ex);
                throw;
            }
        }
        
        private async UniTask<ConfigsCacheModel> FetchConfigsFromServerAsync()
        {
            m_Logger?.Log("Fetching configs from server (parallel)...");

            var athleteTask = m_HttpClientService
                .GetAsync<AthleteTypeConfigsResponse>(ConfigEndpoints.AthleteTypes);

            var exerciseTask = m_HttpClientService
                .GetAsync<ExerciseConfigsResponse>(ConfigEndpoints.Exercises);

            var intensityTask = m_HttpClientService
                .GetAsync<IntensityTypeConfigsResponse>(ConfigEndpoints.Intensities);

            var simulationTask = m_HttpClientService
                .GetAsync<SimulationConfigResponse>(ConfigEndpoints.SimulationConfig);

            var (athleteTypeConfigsResponse, exerciseConfigsResponse,
                    intensityTypeConfigsResponse, simulationConfigResponse) =
                await UniTask.WhenAll(athleteTask, exerciseTask, intensityTask, simulationTask);

            return new ConfigsCacheModel
            {
                AthleteTypes = athleteTypeConfigsResponse.AthleteTypes
                    .Select(AthleteTypeConfigMapper.Map)
                    .ToDictionary(x => x.Type),

                Exercises = exerciseConfigsResponse.Exercises
                    .Select(ExerciseConfigMapper.Map)
                    .ToDictionary(x => x.Type),

                Intensities = intensityTypeConfigsResponse.Intensities
                    .Select(IntensityTypeConfigMapper.Map)
                    .ToDictionary(x => x.Type),

                Simulation = SimulationConfigMapping.Map(simulationConfigResponse)
            };
        }
        
        public AthleteTypeConfig GetAthleteTypeConfig(EAthleteType type)
        {
            if (m_Configs.AthleteTypes.TryGetValue(type, out var result))
            {
                return result;
            }

            throw new KeyNotFoundException($"AthleteType {type} not found");
        }

        public Exercise GetExerciseConfig(EExerciseType type)
        {
            if (m_Configs.Exercises.TryGetValue(type, out var result))
            {
                return result;
            }

            throw new KeyNotFoundException($"Exercise {type} not found");
        }

        public Intensity GetIntensityConfig(EIntensityType type)
        {
            if (m_Configs.Intensities.TryGetValue(type, out var result))
            {
                return result;
            }

            throw new KeyNotFoundException($"Intensity {type} not found");
        }
    }
}