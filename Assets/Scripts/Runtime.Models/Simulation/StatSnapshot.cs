using System;

namespace FormForge.Runtime.Models.Simulation
{
    [Serializable]
    public class StatSnapshot
    {
        public float Strength;
        public float Endurance;
        public float Mobility;
        public float Fatigue;

        public StatSnapshot(float strength, float endurance, float mobility, float fatigue)
        {
            Strength = strength;
            Endurance = endurance;
            Mobility = mobility;
            Fatigue = fatigue;
        }
    }
}