using FormForge.Domain.Exercises;
using FormForge.Networking.Common.Mapping;
using FormForge.Networking.Exercises.DTO;

namespace FormForge.Networking.Exercises.Mapping
{
    public static class ExerciseConfigMapper
    {
        public static Exercise Map(ExerciseConfigDto config)
        {
            return new Exercise
            {
                Type = EExerciseTypeMapper.ToDomain(config.Type),
                DisplayName = config.DisplayName,
                Description = config.Description,
                PrimaryStat = EStatTypeMapper.ToDomain(config.PrimaryStat),
                SecondaryStat = EStatTypeMapper.ToDomain(config.SecondaryStat),
                SecondaryStatWeight = config.SecondaryStatWeight,
                BaseGain = config.BaseGain,
                FatigueCost = config.FatigueCost,
                DurationMinutes = config.DurationMinutes,
                AllowedIntensities = EIntensityTypeMapper.ToDomain(config.AllowedIntensities)
            };
        }
    }
}