using System.Collections.Generic;

namespace FormForge.Configs.DTO
{
    [System.Serializable]
    public class IntensityEnvelopeDto
    {
        public string Version;
        public Dictionary<string, IntensityDto> Intensities;
    }

    [System.Serializable]
    public class IntensityDto
    {
        public float Multiplier;
        public float FatigueMultiplier;
    }
}