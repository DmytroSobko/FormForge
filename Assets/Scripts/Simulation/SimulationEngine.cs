using System.Collections.Generic;
using FormForge.Athletes.Models;
using FormForge.Core.Domain;
using FormForge.TrainingPlans.Models;
using UnityEngine;

namespace FormForge.Simulation
{
    public class SimulationEngine
    {
        private const float RestDayRecovery = 15f;

        public SimulationResult SimulateWeek(AthleteState athlete, TrainingPlan plan)
        {
            var before = CreateSnapshot(athlete);

            float totalPotentialGain = 0f;
            float totalActualGain = 0f;

            var warnings = new List<string>();

            for (int dayIndex = 0; dayIndex < Constants.DaysInWeek; dayIndex++)
            {
                var day = plan.Days[dayIndex];

                if (day.Exercises.Count == 0)
                {
                    RecoverFatigue(athlete);
                    continue;
                }

                foreach (var exercise in day.Exercises)
                {
                    float intensityMultiplier = GetIntensityMultiplier(exercise.Intensity);
                    float fatiguePenalty = FatigueCalculator.CalculatePenalty(athlete.Fatigue, athlete.MaxFatigue);

                    float rawGain = exercise.BaseGain * intensityMultiplier;
                    float finalGain = StatCalculator.ApplyGain(
                        exercise.BaseGain,
                        intensityMultiplier,
                        fatiguePenalty
                    );

                    ApplyStatGain(athlete, exercise.PrimaryStat, finalGain);

                    athlete.Fatigue += exercise.FatigueCost * intensityMultiplier;
                    athlete.Fatigue = Mathf.Min(athlete.Fatigue, athlete.MaxFatigue);

                    totalPotentialGain += rawGain;
                    totalActualGain += finalGain;

                    if (fatiguePenalty > 0.6f)
                    {
                        warnings.Add($"High fatigue reduced gains on day {dayIndex + 1}");
                    }
                }
            }

            var after = CreateSnapshot(athlete);

            float efficiency = totalPotentialGain > 0
                ? totalActualGain / totalPotentialGain
                : 1f;

            return new SimulationResult(athlete.CurrentWeek, before, after, efficiency, warnings);
        }
        
        private void RecoverFatigue(AthleteState athlete)
        {
            athlete.Fatigue -= RestDayRecovery;
            athlete.Fatigue = Mathf.Max(athlete.Fatigue, 0f);
        }

        private void ApplyStatGain(AthleteState athlete, EStatType stat, float value)
        {
            switch (stat)
            {
                case EStatType.Strength:
                    athlete.Strength += value;
                    break;
                case EStatType.Endurance:
                    athlete.Endurance += value;
                    break;
                case EStatType.Mobility:
                    athlete.Mobility += value;
                    break;
            }
        }

        private float GetIntensityMultiplier(EIntensityType intensity)
        {
            return intensity switch
            {
                EIntensityType.Low => 0.6f,
                EIntensityType.Medium => 1.0f,
                EIntensityType.High => 1.4f,
                _ => 1.0f
            };
        }

        private StatSnapshot CreateSnapshot(AthleteState athlete)
        {
            return new StatSnapshot(athlete.Strength, athlete.Endurance, athlete.Mobility, athlete.Fatigue);
        }
    }
}
