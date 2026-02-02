using System.Collections.Generic;
using FormForge.Athletes.Models;
using FormForge.Configs.Runtime;
using FormForge.Core.Services;
using FormForge.Services;
using FormForge.TrainingPlans.Models;
using UnityEngine;

namespace FormForge.Simulation
{
    public class SimulationEngine
    {
        private readonly IConfigService m_ConfigService;
        private readonly SimulationConfig m_SimulationConfig;

        public SimulationEngine()
        {
            m_ConfigService = ServiceLocator.GetService<IConfigService>();
            
            m_SimulationConfig = m_ConfigService.Simulation;
        }

        public SimulationResult SimulateWeek(AthleteState athlete, TrainingPlan plan)
        {
            var before = athlete.Snapshot();

            float totalPotential = 0f;
            float totalActual = 0f;
            var warnings = new List<string>();

            foreach (var day in plan.Days)
            {
                // Rest day
                if (day.Exercises.Count == 0)
                {
                    athlete.Fatigue = Mathf.Max(athlete.Fatigue - m_SimulationConfig.RestDayRecovery, 0f);
                    continue;
                }

                foreach (var planned in day.Exercises)
                {
                    var exercise = planned.Exercise;
                    var intensityConfig = m_ConfigService.Intensities[planned.Intensity];

                    float fatigueRatio = athlete.Fatigue / athlete.MaxFatigue;
                    float penalty = Mathf.Clamp(fatigueRatio, 0f, m_SimulationConfig.MaxFatiguePenalty);

                    float rawGain = exercise.BaseGain * intensityConfig.Multiplier;
                    float finalGain = rawGain * (1f - penalty);

                    athlete.ApplyGain(exercise.PrimaryStat, finalGain);

                    athlete.Fatigue += exercise.FatigueCost * intensityConfig.FatigueMultiplier;
                    athlete.Fatigue = Mathf.Min(athlete.Fatigue, athlete.MaxFatigue);

                    totalPotential += rawGain;
                    totalActual += finalGain;

                    if (penalty > m_SimulationConfig.HighFatigueThreshold)
                    {
                        warnings.Add("High fatigue reduced gains");
                    }
                }
            }

            var after = athlete.Snapshot();

            float efficiency = totalPotential > 0f
                ? totalActual / totalPotential
                : 1f;

            return new SimulationResult(before, after, efficiency, warnings);
        }
    }
}
