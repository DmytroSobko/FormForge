using System.Collections.Generic;

namespace FormForge.Networking.AthleteTypeConfigs.DTO
{
    [System.Serializable]
    public class AthleteTypeConfigsResponse
    {
        public string Version;
        public List<AthleteTypeConfigDto> AthleteTypes;
    }
}