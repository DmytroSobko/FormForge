using System.Collections.Generic;
using System.Linq;
using FormForge.Domain.Athletes;
using FormForge.Networking.Common.DTO;

namespace FormForge.Networking.Common.Mapping
{
    public static class EAthleteTypeMapper
    {
        private static readonly Dictionary<EAthleteType, EAthleteTypeDto> DomainToDto =
            new Dictionary<EAthleteType, EAthleteTypeDto>
            {
                { EAthleteType.None, EAthleteTypeDto.none },
                { EAthleteType.Balanced, EAthleteTypeDto.balanced },
                { EAthleteType.StrengthFocused, EAthleteTypeDto.strength_focused },
                { EAthleteType.EnduranceFocused, EAthleteTypeDto.endurance_focused }
            };

        private static readonly Dictionary<EAthleteTypeDto, EAthleteType> DtoToDomain =
            DomainToDto.ToDictionary(x => 
                x.Value, x => x.Key);

        public static EAthleteTypeDto ToDto(EAthleteType domain)
            => DomainToDto[domain];

        public static EAthleteType ToDomain(EAthleteTypeDto dto)
            => DtoToDomain[dto];
    }
}