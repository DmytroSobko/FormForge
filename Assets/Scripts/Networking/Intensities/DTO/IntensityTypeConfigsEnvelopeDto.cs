using System.Collections.Generic;

namespace FormForge.Networking.Intensities.DTO
{
    [System.Serializable]
    public class IntensityTypeConfigsEnvelopeDto
    {
        public string Version;
        public List<IntensityTypeConfigDto> Intensities;
    }
}