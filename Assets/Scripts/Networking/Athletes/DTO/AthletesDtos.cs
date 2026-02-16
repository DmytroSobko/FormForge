using System.Collections.Generic;

namespace FormForge.Networking.Athletes.DTO
{
    [System.Serializable]
    public class AthletesEnvelopeDto
    {
        public List<AthleteDto> Athletes;
    }

    [System.Serializable]
    public class AthleteDto
    {
        public string Id;
        public string DisplayName;
    }
}