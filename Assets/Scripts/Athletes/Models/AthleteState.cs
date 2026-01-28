using FormForge.Core.Domain;
using FormForge.Simulation;

namespace FormForge.Athletes.Models
{
    public class AthleteState
    {
        public float Strength { get; set; }
        public float Endurance { get; set; }
        public float Mobility { get; set; }
        public float Fatigue { get; set; }
        public float MaxFatigue { get; set; }
        public int CurrentWeek { get; set; }
        
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