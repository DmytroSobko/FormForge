using FormForge.Domain.Intensities;
using FormForge.Networking.Common.Mapping;
using FormForge.Networking.Intensities.DTO;

namespace FormForge.Networking.Intensities.Mapping
{
    public static class IntensityTypeConfigMapper
    {
        public static Intensity Map(IntensityTypeConfigDto typeConfigDto)
        {
            return new Intensity
            {
                Type = EIntensityTypeMapper.ToDomain(typeConfigDto.Type),
                Multiplier = typeConfigDto.Multiplier,
                FatigueMultiplier = typeConfigDto.FatigueMultiplier
            };
        }
    }
}