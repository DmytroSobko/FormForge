using FormForge.Networking.Common.DTO;

namespace FormForge.Networking.Athletes.DTO
{
    [System.Serializable]
    public class AthleteDto
    {
        public string Id;
        public EAthleteTypeDto Type;
        public string DisplayName;
    }
}