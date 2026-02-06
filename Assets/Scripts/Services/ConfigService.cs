using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Configs.DTO;
using FormForge.Configs.Runtime;
using FormForge.Core.Config;
using FormForge.Core.Domain;
using FormForge.Core.Services;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Networking;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services
{
    public class ConfigService : IConfigService
    {
        private readonly IHttpClientService m_HttpClient;

        private ILogger m_Logger = new UnityLogger(nameof(ConfigService));

        public IReadOnlyDictionary<string, AthleteType> AthleteTypes { get; private set; }
        public IReadOnlyDictionary<string, Exercise> Exercises { get; private set; }
        public IReadOnlyDictionary<EIntensityType, Intensity> Intensities { get; private set; }
        public SimulationConfig Simulation { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IConfigService, ConfigService>(ServiceLifespan.LazySingleton);
        }
        
        public ConfigService()
        {
            m_HttpClient = ServiceLocator.GetService<IHttpClientService>();
        }

        public async Task LoadConfigsAsync()
        {
            m_Logger?.Log("Starting config load");

            try
            {
                m_Logger?.Log("Fetching athlete types...");
                var athleteDto = await m_HttpClient.GetAsync<AthleteTypesEnvelopeDto>(
                    ApiEndpoints.AthleteTypes);

                m_Logger?.Log($"Athlete types loaded: {athleteDto.Athletes.Count}");

                m_Logger?.Log("Fetching exercises...");
                var exerciseDto = await m_HttpClient.GetAsync<ExerciseEnvelopeDto>(
                    ApiEndpoints.Exercises);

                m_Logger?.Log($"Exercises loaded: {exerciseDto.Exercises.Count}");

                m_Logger?.Log("Fetching intensities...");
                var intensityDto = await m_HttpClient.GetAsync<IntensityEnvelopeDto>(
                    ApiEndpoints.Intensities);

                m_Logger?.Log($"Intensities loaded: {intensityDto.Intensities.Count}");

                m_Logger?.Log("Fetching simulation config...");
                var simDto = await m_HttpClient.GetAsync<SimulationConfigEnvelopeDto>(
                    ApiEndpoints.SimulationConfig);

                m_Logger?.Log($"Simulation config loaded. Version: {simDto.Version}");

                AthleteTypes = MapById(athleteDto.Athletes, ConfigMapper.Map);
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