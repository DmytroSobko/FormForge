using System;
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
        {
            if (DomainToDto.TryGetValue(domain, out var result))
            {
                return result;
            }

            throw new ArgumentOutOfRangeException(nameof(domain), domain, $"No DTO mapping defined for {domain}");
        }

        public static EIntensityType ToDomain(EIntensityTypeDto dto)
        {
            if (DtoToDomain.TryGetValue(dto, out var result))
                return result;

            throw new ArgumentOutOfRangeException(nameof(dto), dto, $"No Domain mapping defined for {dto}");
        }

        public static HashSet<EIntensityTypeDto> ToDto(HashSet<EIntensityType> domain)
        {
            if (domain == null || domain.Count == 0)
            {
                return new HashSet<EIntensityTypeDto>();
            }

            var result = new HashSet<EIntensityTypeDto>();
            foreach (var item in domain)
            {
                result.Add(ToDto(item));
            }
            return result;
        }

        public static HashSet<EIntensityType> ToDomain(HashSet<EIntensityTypeDto> dto)
        {
            if (dto == null || dto.Count == 0)
            {
                return new HashSet<EIntensityType>();
            }
            
            var result = new HashSet<EIntensityType>();
            foreach (var item in dto)
            {
                result.Add(ToDomain(item));
            }
            return result;
        }
    }
}