using System.Collections.Generic;
using FormForge.Domain.Athletes;
using FormForge.UI.Tooltip.Models;

namespace FormForge.UI.Tooltip.Factories
{
    public static class AthleteTooltipFactory
    {
        public static TooltipData Create(Athlete athlete)
        {
            return new TooltipData
            {
                Title = athlete.Name,
                Description = athlete.Type.ToString(),
                Stats = new List<TooltipStat>
                {
                    new TooltipStat 
                    { 
                        Title = "Strength", 
                        Value = $"{athlete.Strength}"
                    },
                    new TooltipStat
                    {
                        Title = "Endurance", 
                        Value = $"{athlete.Endurance}"
                    },
                    new TooltipStat
                    {
                        Title = "Mobility", 
                        Value = $"{athlete.Mobility}"
                    },
                    new TooltipStat
                    {
                        Title = "Fatigue", 
                        Value = $"{athlete.Fatigue}/{athlete.MaxFatigue}"
                    },
                    new TooltipStat
                    {
                        Title = "Week", 
                        Value = $"{athlete.Week}"
                    }
                }
            };
        }
        
        public static TooltipData Create(AthleteTypeConfig config)
        {
            return new TooltipData
            {
                Title = config.DisplayName,
                Description = config.Description,
                Stats = new List<TooltipStat>
                {
                    new TooltipStat 
                    { 
                        Title = "Strength", 
                        Value = $"{config.StatBlock.Strength}" 
                    },
                    new TooltipStat
                    {
                        Title = "Endurance", 
                        Value = $"{config.StatBlock.Endurance}"
                    },
                    new TooltipStat
                    {
                        Title = "Mobility", 
                        Value = $"{config.StatBlock.Mobility}"
                    },
                    new TooltipStat
                    {
                        Title = "Max Fatigue", 
                        Value = $"{config.MaxFatigue}"
                    },
                    new TooltipStat
                    {
                        Title = "Recovery Multiplier", 
                        Value = $"{config.RecoveryMultiplier}"
                    },
                    new TooltipStat
                    {
                        Title = "Fatigue Sensitivity", 
                        Value = $"{config.FatigueSensitivity}"
                    }
                }
            };
        }
    }
}