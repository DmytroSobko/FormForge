using FormForge.UI.Tooltip.Models;
using TMPro;
using UnityEngine;

namespace FormForge.UI.Tooltip
{
    public class TooltipStatRow: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TitleText;
        [SerializeField] private TextMeshProUGUI m_ValueText;

        public void Init(TooltipStat stat)
        {
            m_TitleText.text = stat.Title;
            m_ValueText.text = stat.Value;
        }
    }
}