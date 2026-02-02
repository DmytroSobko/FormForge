using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FormForge.Configs.DTO;
using FormForge.Configs.Runtime;
using FormForge.Core.Domain;
using FormForge.Infrastructure.Networking;

namespace FormForge.Services
{
    public class ConfigService : IConfigService
    {
        private readonly HttpClientService m_HttpClient;
        private const string BaseUrl = "http://localhost:8080/api/config";

        public IReadOnlyDictionary<string, AthleteType> AthleteTypes { get; private set; }
        public IReadOnlyDictionary<string, Exercise> Exercises { get; private set; }
        public IReadOnlyDictionary<EIntensityType, Intensity> Intensities { get; private set; }
        public SimulationConfig Simulation { get; private set; }

        public ConfigService(HttpClientService httpClient)
        {
            m_HttpClient = httpClient;
        }

        public async Task LoadConfigsAsync()
        {
            var athleteDto = await m_HttpClient.GetAsync<AthleteTypesEnvelopeDto>($"{BaseUrl}/athletes");
            var exerciseDto = await m_HttpClient.GetAsync<ExerciseEnvelopeDto>($"{BaseUrl}/exercises");
            var intensityDto = await m_HttpClient.GetAsync<IntensityEnvelopeDto>($"{BaseUrl}/intensities");
            var simDto = await m_HttpClient.GetAsync<SimulationConfigEnvelopeDto>($"{BaseUrl}/simulation");

            AthleteTypes = MapById(athleteDto.Athletes, ConfigMapper.Map);
            Exercises = MapById(exerciseDto.Exercises, ConfigMapper.Map);
            Intensities = MapIntensities(intensityDto.Intensities);
            Simulation = ConfigMapper.Map(simDto.Simulation);
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