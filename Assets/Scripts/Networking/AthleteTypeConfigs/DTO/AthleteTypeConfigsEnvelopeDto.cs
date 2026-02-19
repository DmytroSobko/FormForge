using System.Collections.Generic;

namespace FormForge.Networking.Configs.DTO
{
    [System.Serializable]
    public class AthleteTypeConfigsEnvelopeDto
    {
        public string Version;
        public List<AthleteTypeConfigDto> AthleteTypes;
    }
}