using FormForge.Networking.Common.DTO;

namespace FormForge.Networking.Intensities.DTO
{
    [System.Serializable]
    public class IntensityTypeConfigDto
    {
        public EIntensityTypeDto Type;
        public float Multiplier;
        public float FatigueMultiplier;
    }
}