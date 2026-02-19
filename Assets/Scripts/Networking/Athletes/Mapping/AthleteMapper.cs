using FormForge.Core.Networking.AthleteTypes.Mapping;
using FormForge.Domain.Athletes;
using FormForge.Networking.Athletes.DTO;

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
                DisplayName = dto.DisplayName,
            };
        }
    }
}