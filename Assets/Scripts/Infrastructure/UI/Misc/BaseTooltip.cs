using FormForge.Infrastructure.UI.Overlays;
using UnityEngine;

namespace FormForge.Infrastructure.UI.Misc
{
    public abstract class BaseTooltip : FadableOverlayBase
    {
        protected const float ScreenPadding = 10f;
        protected static readonly Vector2 Offset = new Vector2(20f, -20f);

        [SerializeField] private RectTransform m_Rect;

        protected void PositionTooltip(Vector2 screenPos)
        {
            Vector2 pos = screenPos + Offset;

            var rect = m_Rect.rect;
            float width = rect.width;
            float height = rect.height;

            float x = Mathf.Clamp(pos.x, ScreenPadding, Screen.width - width - ScreenPadding);
            float y = Mathf.Clamp(pos.y, height + ScreenPadding, Screen.height - ScreenPadding);

            m_Rect.position = new Vector2(x, y);
        }
    }
}