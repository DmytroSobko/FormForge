using System.Collections.Generic;

namespace FormForge.Networking.Intensities.DTO
{
    [System.Serializable]
    public class IntensityTypeConfigsResponse
    {
        public string Version;
        public List<IntensityTypeConfigDto> Intensities;
    }
}