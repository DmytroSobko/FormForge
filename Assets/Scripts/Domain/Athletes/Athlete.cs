using System;
using FormForge.Domain.Simulation;

namespace FormForge.Domain.Athletes
{
    [Serializable]
    public class Athlete
    {
        public EAthleteType Type { get; set; }
        
        public string Id { get; set; }
        public string Name { get; set; }
        public float Strength { get; set; }
        public float Endurance { get; set; }
        public float Mobility { get; set; }
        public float Fatigue { get; set; }
        public float MaxFatigue { get; set; }
        public int Week { get; set; }

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