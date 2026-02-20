using FormForge.Domain.Exercises;
using FormForge.Networking.Common.Mapping;
using FormForge.Networking.Exercises.DTO;

namespace FormForge.Networking.Exercises.Mapping
{
    public static class ExerciseConfigMapper
    {
        public static Exercise Map(ExerciseConfigDto configDto)
        {
            return new Exercise
            {
                Type = EExerciseTypeMapper.ToDomain(configDto.Type),
                DisplayName = configDto.DisplayName,
                Description = configDto.Description,
                PrimaryStat = EStatTypeMapper.ToDomain(configDto.PrimaryStat),
                SecondaryStat = EStatTypeMapper.ToDomain(configDto.SecondaryStat),
                SecondaryStatWeight = configDto.SecondaryStatWeight,
                BaseGain = configDto.BaseGain,
                FatigueCost = configDto.FatigueCost,
                DurationMinutes = configDto.DurationMinutes,
                AllowedIntensities = EIntensityTypeMapper.ToDomain(configDto.AllowedIntensities)
            };
        }
    }
}