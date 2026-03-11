using FormForge.UI.Tooltip.Models;
using UnityEngine;

namespace FormForge.UI.Tooltip.Messages
{
    public class StatsTooltipShowMessage
    {
        public TooltipData TooltipData
        {
            get;
        }
        
        public Vector2 ScreenPos 
        {
            get;
        }
        
        public StatsTooltipShowMessage(TooltipData data, Vector2 screenPos)
        {
            TooltipData = data;
            ScreenPos = screenPos;
        }
    }
}