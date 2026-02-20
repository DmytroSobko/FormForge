using System.Collections.Generic;
using System.Linq;
using FormForge.Domain.Intensities;
using FormForge.Networking.Common.DTO;

namespace FormForge.Networking.Common.Mapping
{
    public static class EIntensityTypeMapper
    {
        private static readonly Dictionary<EIntensityType, EIntensityTypeDto> DomainToDto =
            new Dictionary<EIntensityType, EIntensityTypeDto>
            {
                { EIntensityType.None, EIntensityTypeDto.none },
                { EIntensityType.Low, EIntensityTypeDto.low },
                { EIntensityType.Medium, EIntensityTypeDto.medium },
                { EIntensityType.High, EIntensityTypeDto.high }
            };

        private static readonly Dictionary<EIntensityTypeDto, EIntensityType> DtoToDomain =
            DomainToDto.ToDictionary(x => 
                x.Value, x => x.Key);

        public static EIntensityTypeDto ToDto(EIntensityType domain)
            => DomainToDto[domain];

        public static EIntensityType ToDomain(EIntensityTypeDto dto)
            => DtoToDomain[dto];


        public static HashSet<EIntensityTypeDto> ToDto(HashSet<EIntensityType> domain)
            => domain.Select(ToDto).ToHashSet();
        
        public static HashSet<EIntensityType> ToDomain(HashSet<EIntensityTypeDto> dto)
            => dto.Select(ToDomain).ToHashSet();
    }
}