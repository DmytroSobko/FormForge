using FormForge.Domain.Athletes;
using FormForge.Networking.Athletes.DTO;
using FormForge.Networking.Common.Mapping;

namespace FormForge.Networking.Athletes.Mapping
{
    public static class AthleteMapper
    {
        public static Athlete Map(AthleteDto dto)
        {
            return new Athlete
            {
                Id = dto.Id,
                Type = EAthleteTypeMapper.ToDomain(dto.Type),
                Name = dto.Name,
                Strength = dto.Strength,
                Endurance = dto.Endurance,
                Mobility = dto.Mobility,
                Fatigue = dto.Fatigue,
                MaxFatigue = dto.MaxFatigue,
                Week = dto.Week
            };
        }
    }
}