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
    }
}