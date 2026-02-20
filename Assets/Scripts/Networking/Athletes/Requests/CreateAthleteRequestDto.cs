using System;
using FormForge.Domain.Athletes;
using FormForge.Networking.Common.DTO;
using FormForge.Networking.Common.Mapping;

namespace FormForge.Networking.Athletes.Requests
{
    [Serializable]
    public class CreateAthleteRequestDto
    {
        public EAthleteTypeDto Type;
        public string Name;

        public CreateAthleteRequestDto(EAthleteType type, string name)
        {
            Type = EAthleteTypeMapper.ToDto(type);
            Name = name;
        }
    }
}