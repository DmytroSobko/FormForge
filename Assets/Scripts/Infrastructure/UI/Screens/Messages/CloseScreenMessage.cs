using System;

namespace FormForge.Infrastructure.UI.Screens.Messages
{
    public class CloseScreenMessage
    {
        public Type ScreenType
        {
            get;
        }

        public CloseScreenMessage(Type screenType)
        {
            ScreenType = screenType;
        }
    }
}