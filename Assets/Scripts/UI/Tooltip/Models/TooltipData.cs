using System.Collections.Generic;

namespace FormForge.UI.Tooltip.Models
{
    public class TooltipData
    {
        public string Title;
        public string Description;
        public List<TooltipStat> Stats = new List<TooltipStat>();
    }
}