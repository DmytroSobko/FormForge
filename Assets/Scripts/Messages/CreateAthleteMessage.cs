using FormForge.Domain.Athletes;

namespace FormForge.Messages
{
    public class CreateAthleteMessage
    {
        public string AthleteName
        {
            get;
        }
        
        public EAthleteType AthleteType 
        {
            get;
        }
        
        public CreateAthleteMessage(string athleteName, EAthleteType athleteType)
        {
            AthleteName = athleteName;
            AthleteType = athleteType;
        }
    }
}