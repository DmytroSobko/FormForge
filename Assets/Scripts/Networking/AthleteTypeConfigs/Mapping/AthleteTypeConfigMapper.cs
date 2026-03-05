using FormForge.Domain.Athletes;
using FormForge.Networking.AthleteTypeConfigs.DTO;
using FormForge.Networking.Common.Mapping;

namespace FormForge.Networking.AthleteTypeConfigs.Mapping
{
    public static class AthleteTypeConfigMapper
    {
        public static AthleteTypeConfig Map(AthleteTypeConfigDto config)
        {
            return new AthleteTypeConfig
            {
                Type = EAthleteTypeMapper.ToDomain(config.Type),
                DisplayName = config.DisplayName,
                Description = config.Description,
                StatBlock = new StatBlock
                {
                    Strength = config.BaseStats.Strength,
                    Endurance = config.BaseStats.Endurance,
                    Mobility = config.BaseStats.Mobility,                        
                },
                MaxFatigue = config.MaxFatigue,
                RecoveryMultiplier = config.RecoveryMultiplier,
                FatigueSensitivity = config.FatigueSensitivity
            };
        }
    }
}