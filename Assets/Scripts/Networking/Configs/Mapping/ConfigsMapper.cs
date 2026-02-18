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
                Type = MapAthleteType(dto.Type),
                DisplayName = dto.DisplayName,
                Description = dto.Description,
                StatBlock = new StatBlock
                {
                    Strength = dto.BaseStats.Strength,
                    Endurance = dto.BaseStats.Endurance,
                    Mobility = dto.BaseStats.Mobility,                        
                },
                MaxFatigue = dto.MaxFatigue,
                RecoveryMultiplier = dto.RecoveryMultiplier,
                FatigueSensitivity = dto.FatigueSensitivity
            };
        }

        private static EAthleteType MapAthleteType(string type)
        {
            return type switch
            {
                "balanced" => EAthleteType.Balanced,
                "endurance_focused" => EAthleteType.EnduranceFocused,
                "strength_focused" => EAthleteType.StrengthFocused,
                _ => EAthleteType.None
            };
        }

        public static Exercise Map(ExerciseDto dto)
        {
            return new Exercise
            {
                Type = GetExerciseType(dto.Type),
                DisplayName = dto.DisplayName,
                Description = dto.Description,
                PrimaryStat = MapStatType(dto.PrimaryStat),
                SecondaryStat = string.IsNullOrEmpty(dto.SecondaryStat)
                    ? EStatType.None
                    : MapStatType(dto.SecondaryStat),
                SecondaryStatWeight = dto.SecondaryStatWeight,
                BaseGain = dto.BaseGain,
                FatigueCost = dto.FatigueCost,
                DurationMinutes = dto.DurationMinutes,
                AllowedIntensities = MapIntensityTypes(dto.AllowedIntensities)
            };
        }
        
        private static EExerciseType GetExerciseType(string type)
        {
            return type switch
            {
                "bench_press" => EExerciseType.BenchPress,
                "squat" => EExerciseType.Squat,
                "deadlift" => EExerciseType.Deadlift,
                "overhead_press" => EExerciseType.OverheadPress,
                "running" => EExerciseType.Running,
                "cycling" => EExerciseType.Cycling,
                "rowing" => EExerciseType.Rowing,
                "stretching" => EExerciseType.Stretching,
                "yoga_flow" => EExerciseType.YogaFlow,
                "core_stability" => EExerciseType.CoreStability,

                _ => EExerciseType.None
            };
        }

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

        private static EStatType MapStatType(string type)
        {
            return type switch
            {
                "endurance" => EStatType.Endurance,
                "mobility" => EStatType.Mobility,
                "strength" => EStatType.Strength,

                _ => EStatType.None
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
