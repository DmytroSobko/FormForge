using System;
using FormForge.Core.Networking.AthleteTypes.Mapping;
using FormForge.Domain.Athletes;
using FormForge.Networking.Athletes.DTO;

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