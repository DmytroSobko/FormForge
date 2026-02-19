using FormForge.Core.Networking.AthleteTypes.Mapping;
using FormForge.Domain.Exercises;
using FormForge.Networking.Configs.DTO;

namespace FormForge.Core.Networking.Exercises.Mapping
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
                //AllowedIntensities = MapIntensityTypes(configDto.AllowedIntensities)
            };
        }
    }
}