using System.Collections.Generic;
using System.Linq;
using FormForge.Domain;
using FormForge.Networking.Athletes.DTO;

namespace FormForge.Core.Networking.AthleteTypes.Mapping
{
    public static class EStatTypeMapper
    {
        private static readonly Dictionary<EStatType, EStatTypeDto> DomainToDto =
            new Dictionary<EStatType, EStatTypeDto>
            {
                { EStatType.None, EStatTypeDto.none },
                { EStatType.Endurance, EStatTypeDto.endurance },
                { EStatType.Mobility, EStatTypeDto.mobility },
                { EStatType.Strength, EStatTypeDto.strength }
            };

        private static readonly Dictionary<EStatTypeDto, EStatType> DtoToDomain =
            DomainToDto.ToDictionary(x => 
                x.Value, x => x.Key);

        public static EStatTypeDto ToDto(EStatType domain)
            => DomainToDto[domain];

        public static EStatType ToDomain(EStatTypeDto dto)
            => DtoToDomain[dto];
    }
}