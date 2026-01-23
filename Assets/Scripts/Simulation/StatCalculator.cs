namespace FormForge.Simulation
{
    public static class StatCalculator
    {
        public static float ApplyGain(float baseGain, float intensityMultiplier, float fatiguePenalty)
        {
            return baseGain * intensityMultiplier * (1f - fatiguePenalty);
        }
    }
}