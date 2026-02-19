using System.Collections.Generic;
using FormForge.Domain.Intensities;
using FormForge.Domain.Simulation;
using FormForge.Networking.Configs.DTO;

namespace FormForge.Networking.Configs.Mapping
{
    public static class ConfigMapper
    {
        public static Intensity Map(IntensityDto dto)
        {
            return new Intensity
            {
                Type = MapIntensityType(dto.Type),
                Multiplier = dto.Multiplier,
                FatigueMultiplier = dto.FatigueMultiplier
            };
        }

        public static SimulationConfig Map(SimulationConfigDto dto)
        {
            return new SimulationConfig
            {
                RestDayRecovery = dto.RestDayRecovery,
                MaxFatiguePenalty = dto.MaxFatiguePenalty,
                HighFatigueThreshold = dto.HighFatigueThreshold,
            };
        }

        private static HashSet<EIntensityType> MapIntensityTypes(List<string> types)
        {
            var set = new HashSet<EIntensityType>();
            foreach (var type in types)
            {
                set.Add(MapIntensityType(type));
            }
            return set;
        }
        
        private static EIntensityType MapIntensityType(string type)
        {
            return type switch
            {
                "low" => EIntensityType.Low,
                "medium" => EIntensityType.Medium,
                "high" => EIntensityType.High,

                _ => EIntensityType.None
            };
        }
    }
}
