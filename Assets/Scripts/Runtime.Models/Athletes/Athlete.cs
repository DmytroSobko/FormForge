using System;
using FormForge.Domain;
using FormForge.Runtime.Models.Simulation;

namespace FormForge.Runtime.Models.Athletes
{
    [Serializable]
    public class Athlete
    {
        public EAthleteType AthleteType { get; set; }
        public string Id { get; set; }
        public string DisplayName{ get; set; }
        public float Strength { get; set; }
        public float Endurance { get; set; }
        public float Mobility { get; set; }
        public float Fatigue { get; set; }
        public float MaxFatigue { get; set; }
        
        public StatSnapshot Snapshot()
        {
            return new StatSnapshot(Strength, Endurance, Mobility, Fatigue);
        }

        public void ApplyGain(EStatType stat, float value)
        {
            switch (stat)
            {
                case EStatType.Strength:
                    Strength += value;
                    break;
                case EStatType.Endurance:
                    Endurance += value;
                    break;
                case EStatType.Mobility:
                    Mobility += value;
                    break;
            }
        }
    }
}