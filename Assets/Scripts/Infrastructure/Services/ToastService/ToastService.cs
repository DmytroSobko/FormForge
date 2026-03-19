using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Toast;
using UnityEngine;

namespace FormForge.Infrastructure.Services.ToastService
{
    public class ToastService : IToastService
    {
        private readonly IMessageService m_MessageService;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IToastService, ToastService>(ServiceLifespan.LazySingleton);
        }

        public ToastService()
        {
            m_MessageService = ServiceLocator.GetService<IMessageService>();
        }

        public void Show(string toast)
        {
            var message = new ToastShowMessage(toast);
            m_MessageService.Send(message);
        }

        public void Success(string toast)
        {
            var message = new ToastShowMessage(toast, EToastType.Success);
            m_MessageService.Send(message);
        }

        public void Error(string toast)
        {
            var message = new ToastShowMessage(toast, EToastType.Error);
            m_MessageService.Send(message);
        }

        public void Warning(string toast)
        {
            var message = new ToastShowMessage(toast, EToastType.Warning);
            m_MessageService.Send(message);
        }
    }
}