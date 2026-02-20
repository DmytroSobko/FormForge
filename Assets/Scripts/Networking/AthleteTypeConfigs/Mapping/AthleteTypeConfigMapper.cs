using FormForge.Domain.Athletes;
using FormForge.Networking.AthleteTypeConfigs.DTO;
using FormForge.Networking.Common.Mapping;

namespace FormForge.Networking.AthleteTypeConfigs.Mapping
{
    public static class AthleteTypeConfigMapper
    {
        public static AthleteTypeConfig Map(AthleteTypeConfigDto configDto)
        {
            return new AthleteTypeConfig
            {
                Type = EAthleteTypeMapper.ToDomain(configDto.Type),
                DisplayName = configDto.DisplayName,
                Description = configDto.Description,
                StatBlock = new StatBlock
                {
                    Strength = configDto.BaseStats.Strength,
                    Endurance = configDto.BaseStats.Endurance,
                    Mobility = configDto.BaseStats.Mobility,                        
                },
                MaxFatigue = configDto.MaxFatigue,
                RecoveryMultiplier = configDto.RecoveryMultiplier,
                FatigueSensitivity = configDto.FatigueSensitivity
            };
        }
    }
}