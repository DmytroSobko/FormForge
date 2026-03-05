using System;
using FormForge.Domain.Athletes;
using FormForge.Networking.Common.Mapping;
using Newtonsoft.Json;

namespace FormForge.Networking.Athletes.Requests
{
    [Serializable]
    public class CreateAthleteRequestDto
    {
        [JsonProperty("type")]
        public string Type;
        
        [JsonProperty("name")]
        public string Name;

        public CreateAthleteRequestDto(EAthleteType type, string name)
        {
            Type = EAthleteTypeMapper.ToDto(type).ToString();
            Name = name;
        }
    }
}