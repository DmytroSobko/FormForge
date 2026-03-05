using FormForge.Domain.Intensities;
using FormForge.Networking.Common.Mapping;
using FormForge.Networking.Intensities.DTO;

namespace FormForge.Networking.Intensities.Mapping
{
    public static class IntensityTypeConfigMapper
    {
        public static Intensity Map(IntensityTypeConfigDto typeConfig)
        {
            return new Intensity
            {
                Type = EIntensityTypeMapper.ToDomain(typeConfig.Type),
                Multiplier = typeConfig.Multiplier,
                FatigueMultiplier = typeConfig.FatigueMultiplier
            };
        }
    }
}