using FormForge.Networking.Athletes.DTO;
using FormForge.Runtime.Models.Athletes;

namespace FormForge.Networking.Athletes.Mapping
{
    public static class AthletesMapper
    {
        public static Athlete Map(AthleteDto dto)
        {
            return new Athlete
            {
                Id = dto.Id,
                DisplayName = dto.DisplayName,
            };
        }
    }
}