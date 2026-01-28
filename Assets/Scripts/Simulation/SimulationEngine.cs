using System.Collections.Generic;
using FormForge.Athletes.Models;
using FormForge.Configs;
using FormForge.Core.Services;
using FormForge.Services.Simulation;
using FormForge.TrainingPlans.Models;
using UnityEngine;

namespace FormForge.Simulation
{
    public class SimulationEngine
    {
        private readonly SimulationConfig m_SimulationConfig;

        public SimulationEngine()
        {
            m_SimulationConfig = ServiceLocator.GetService<ISimulationService>().Config;
        }

        public SimulationResult SimulateWeek(AthleteState athlete, TrainingPlan plan)
        {
            var before = athlete.Snapshot();

            float totalPotential = 0f;
            float totalActual = 0f;
            var warnings = new List<string>();

            foreach (var day in plan.Days)
            {
                if (day.Exercises.Count == 0)
                {
                    athlete.Fatigue = Mathf.Max(athlete.Fatigue - m_SimulationConfig.RestDayRecovery, 0f);
                    continue;
                }

                foreach (var ex in day.Exercises)
                {
                    string intensityKey = ex.Intensity.ToString().ToLower();
                    float intensity = m_SimulationConfig.IntensityMultipliers[intensityKey];
                    float penalty = Mathf.Clamp(athlete.Fatigue / athlete.MaxFatigue, 0f,
                        m_SimulationConfig.MaxFatiguePenalty);

                    float rawGain = ex.BaseGain * intensity;
                    float finalGain = rawGain * (1f - penalty);

                    athlete.ApplyGain(ex.PrimaryStat, finalGain);

                    athlete.Fatigue += ex.FatigueCost * intensity;
                    athlete.Fatigue = Mathf.Min(
                        athlete.Fatigue,
                        athlete.MaxFatigue
                    );

                    totalPotential += rawGain;
                    totalActual += finalGain;

                    if (penalty > m_SimulationConfig.HighFatigueThreshold)
                    {
                        warnings.Add("High fatigue reduced gains");
                    }
                }
            }

            var after = athlete.Snapshot();

            float efficiency = totalPotential > 0 ? totalActual / totalPotential : 1f;

            return new SimulationResult(before, after, efficiency, warnings);
        }
    }
}
