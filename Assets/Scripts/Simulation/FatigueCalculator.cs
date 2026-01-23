using UnityEngine;

namespace FormForge.Simulation
{
    public static class FatigueCalculator
    {
        public static float CalculatePenalty(float fatigue, float maxFatigue)
        {
            float ratio = fatigue / maxFatigue;
            return Mathf.Clamp(ratio, 0f, 0.7f);
        }
    }
}