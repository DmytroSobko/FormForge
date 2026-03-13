using System.Collections.Generic;
using System.Globalization;
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
                        Value = athlete.Strength.ToString(CultureInfo.CurrentCulture) 
                    },
                    new TooltipStat
                    {
                        Title = "Endurance", 
                        Value = athlete.Endurance.ToString(CultureInfo.CurrentCulture)
                    },
                    new TooltipStat
                    {
                        Title = "Mobility", 
                        Value = athlete.Mobility.ToString(CultureInfo.InvariantCulture)
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
                        Value = config.StatBlock.Strength.ToString(CultureInfo.CurrentCulture) 
                    },
                    new TooltipStat
                    {
                        Title = "Endurance", 
                        Value = config.StatBlock.Endurance.ToString(CultureInfo.CurrentCulture)
                    },
                    new TooltipStat
                    {
                        Title = "Mobility", 
                        Value = config.StatBlock.Mobility.ToString(CultureInfo.InvariantCulture)
                    }
                }
            };
        }
    }
}