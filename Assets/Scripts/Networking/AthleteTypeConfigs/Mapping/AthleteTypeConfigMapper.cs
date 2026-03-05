using FormForge.Domain.Athletes;
using FormForge.Networking.AthleteTypeConfigs.DTO;
using FormForge.Networking.Common.Mapping;
using StatBlock = FormForge.Domain.Athletes.StatBlock;

namespace FormForge.Networking.AthleteTypeConfigs.Mapping
{
    public static class AthleteTypeConfigMapper
    {
        public static Domain.Athletes.AthleteTypeConfig Map(AthleteTypeConfigDto config)
        {
            return new Domain.Athletes.AthleteTypeConfig
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