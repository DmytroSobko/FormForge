using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FormForge.Core.Networking;
using FormForge.Core.Networking.AthleteTypeConfigs.Mapping;
using FormForge.Core.Networking.Exercises.Mapping;
using FormForge.Domain.Athletes;
using FormForge.Domain.Exercises;
using FormForge.Domain.Intensities;
using FormForge.Domain.Simulation;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.CacheService;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.HttpClientService;
using FormForge.Networking.Configs.DTO;
using FormForge.Networking.Configs.Mapping;
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
                .GetAsync<AthleteTypeConfigsEnvelopeDto>(APIEndpoints.Configs.AthleteTypes);

            var exerciseTask = m_HttpClientService
                .GetAsync<ExerciseConfigsEnvelopeDto>(APIEndpoints.Configs.Exercises);

            var intensityTask = m_HttpClientService
                .GetAsync<IntensityTypesEnvelopeDto>(APIEndpoints.Configs.Intensities);

            var simulationTask = m_HttpClientService
                .GetAsync<SimulationConfigEnvelopeDto>(APIEndpoints.Configs.SimulationConfig);

            await UniTask.WhenAll(athleteTask, exerciseTask, intensityTask, simulationTask);

            var athleteTypesEnvelopeDto = await athleteTask;
            var exercisesEnvelopeDto = await exerciseTask;
            var intensityTypesEnvelopeDto = await intensityTask;
            var simulationConfigEnvelopeDto = await simulationTask;

            return new ConfigsCacheModel
            {
                AthleteTypes = athleteTypesEnvelopeDto.AthleteTypes
                    .Select(AthleteTypeConfigMapper.Map)
                    .ToDictionary(x => x.Type),

                Exercises = exercisesEnvelopeDto.Exercises
                    .Select(ExerciseConfigMapper.Map)
                    .ToDictionary(x => x.Type),

                Intensities = intensityTypesEnvelopeDto.Intensities
                    .Select(ConfigMapper.Map)
                    .ToDictionary(x => x.Type),

                Simulation = ConfigMapper.Map(simulationConfigEnvelopeDto.Simulation)
            };
        }
        
        public AthleteTypeConfig GetAthleteType(EAthleteType type)
        {
            if (m_Configs.AthleteTypes.TryGetValue(type, out var result))
            {
                return result;
            }

            throw new KeyNotFoundException($"AthleteType {type} not found");
        }

        public Exercise GetExercise(EExerciseType type)
        {
            if (m_Configs.Exercises.TryGetValue(type, out var result))
            {
                return result;
            }

            throw new KeyNotFoundException($"Exercise {type} not found");
        }

        public Intensity GetIntensity(EIntensityType type)
        {
            if (m_Configs.Intensities.TryGetValue(type, out var result))
            {
                return result;
            }

            throw new KeyNotFoundException($"Intensity {type} not found");
        }
    }
}