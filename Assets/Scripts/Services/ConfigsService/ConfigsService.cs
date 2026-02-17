using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.Core.Networking;
using FormForge.Domain;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.HttpClientService;
using FormForge.Networking.Configs.DTO;
using FormForge.Networking.Configs.Mapping;
using FormForge.Runtime.Models.Athletes;
using FormForge.Runtime.Models.Exercises;
using FormForge.Runtime.Models.Intensities;
using FormForge.Runtime.Models.Simulation;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.ConfigsService
{
    public class ConfigsService : IConfigsService
    {
        public IReadOnlyDictionary<string, AthleteType> AthleteTypes { get; private set; }
        public IReadOnlyDictionary<string, Exercise> Exercises { get; private set; }
        public IReadOnlyDictionary<EIntensityType, Intensity> Intensities { get; private set; }
        public SimulationConfig Simulation { get; private set; }
        
        private ILogger m_Logger = new UnityLogger(nameof(ConfigsService));
        
        private readonly IHttpClientService m_HttpClientService;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IConfigsService, ConfigsService>(ServiceLifespan.LazySingleton);
        }
        
        public ConfigsService()
        {
            m_HttpClientService = ServiceLocator.GetService<IHttpClientService>();
        }

        public async UniTask LoadConfigsAsync()
        {
            m_Logger?.Log("Starting config load");

            try
            {
                m_Logger?.Log("Fetching athlete types...");
                var athleteDto = await m_HttpClientService.GetAsync<AthleteTypesEnvelopeDto>(
                    APIEndpoints.Configs.AthleteTypes);

                m_Logger?.Log($"Athlete types loaded: {athleteDto.AthleteTypes.Count}");

                m_Logger?.Log("Fetching exercises...");
                var exerciseDto = await m_HttpClientService.GetAsync<ExerciseEnvelopeDto>(
                    APIEndpoints.Configs.Exercises);

                m_Logger?.Log($"Exercises loaded: {exerciseDto.Exercises.Count}");

                m_Logger?.Log("Fetching intensities...");
                var intensityDto = await m_HttpClientService.GetAsync<IntensityEnvelopeDto>(
                    APIEndpoints.Configs.Intensities);

                m_Logger?.Log($"Intensities loaded: {intensityDto.Intensities.Count}");

                m_Logger?.Log("Fetching simulation config...");
                var simDto = await m_HttpClientService.GetAsync<SimulationConfigEnvelopeDto>(
                    APIEndpoints.Configs.SimulationConfig);

                m_Logger?.Log($"Simulation config loaded. Version: {simDto.Version}");

                AthleteTypes = MapById(athleteDto.AthleteTypes, ConfigMapper.Map);
                Exercises = MapById(exerciseDto.Exercises, ConfigMapper.Map);
                Intensities = MapIntensities(intensityDto.Intensities);
                Simulation = ConfigMapper.Map(simDto.Simulation);

                m_Logger?.Log("All configs loaded and mapped successfully");
            }
            catch (Exception ex)
            {
                m_Logger?.LogError("Failed to load configs");
                m_Logger?.LogException(ex);
                throw;
            }
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

        private static Dictionary<EIntensityType, Intensity> MapIntensities(Dictionary<string, IntensityDto> source)
        {
            var dict = new Dictionary<EIntensityType, Intensity>();
            foreach (var kv in source)
            {
                dict[Enum.Parse<EIntensityType>(kv.Key, true)] = ConfigMapper.Map(kv.Value);
            }
            return dict;
        }
    }
}