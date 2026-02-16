using System;
using System.Collections.Generic;
using FormForge.Domain;
using FormForge.Networking.Configs.DTO;
using FormForge.Runtime.Models.Athletes;
using FormForge.Runtime.Models.Exercises;
using FormForge.Runtime.Models.Intensities;
using FormForge.Runtime.Models.Simulation;

namespace FormForge.Networking.Configs.Mapping
{
    public static class ConfigMapper
    {
        public static AthleteType Map(AthleteTypeDto dto)
        {
            return new AthleteType
            {
                Id = dto.Id,
                DisplayName = dto.DisplayName,
                Description = dto.Description,
                Strength = dto.BaseStats.Strength,
                Endurance = dto.BaseStats.Endurance,
                Mobility = dto.BaseStats.Mobility,
                MaxFatigue = dto.MaxFatigue,
                RecoveryMultiplier = dto.RecoveryMultiplier,
                FatigueSensitivity = dto.FatigueSensitivity
            };
        }

        public static Exercise Map(ExerciseDto dto)
        {
            return new Exercise
            {
                Id = dto.Id,
                DisplayName = dto.DisplayName,
                Description = dto.Description,
                PrimaryStat = ParseStat(dto.PrimaryStat),
                SecondaryStat = string.IsNullOrEmpty(dto.SecondaryStat)
                    ? EStatType.None
                    : ParseStat(dto.SecondaryStat),
                SecondaryStatWeight = dto.SecondaryStatWeight,
                BaseGain = dto.BaseGain,
                FatigueCost = dto.FatigueCost,
                DurationMinutes = dto.DurationMinutes,
                AllowedIntensities = ParseIntensities(dto.AllowedIntensities)
            };
        }

        public static Intensity Map(IntensityDto dto)
        {
            return new Intensity
            {
                Multiplier = dto.Multiplier,
                FatigueMultiplier = dto.FatigueMultiplier
            };
        }

        public static SimulationConfig Map(SimulationConfigDto dto)
        {
            return new SimulationConfig
            {
                DaysInWeek = dto.DaysInWeek,
                RestDayRecovery = dto.RestDayRecovery,
                MaxFatiguePenalty = dto.MaxFatiguePenalty,
                HighFatigueThreshold = dto.HighFatigueThreshold
            };
        }

        private static EStatType ParseStat(string value)
        {
            return Enum.Parse<EStatType>(value, true);
        }

        private static HashSet<EIntensityType> ParseIntensities(List<string> values)
        {
            var set = new HashSet<EIntensityType>();
            foreach (var v in values)
                set.Add(Enum.Parse<EIntensityType>(v, true));
            return set;
        }
    }
}
