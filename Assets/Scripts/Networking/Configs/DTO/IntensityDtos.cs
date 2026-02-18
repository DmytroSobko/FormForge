using System.Collections.Generic;

namespace FormForge.Networking.Configs.DTO
{
    [System.Serializable]
    public class IntensityTypesEnvelopeDto
    {
        public string Version;
        public List<IntensityDto> Intensities;
    }

    [System.Serializable]
    public class IntensityDto
    {
        public string Type;
        public float Multiplier;
        public float FatigueMultiplier;
    }
}