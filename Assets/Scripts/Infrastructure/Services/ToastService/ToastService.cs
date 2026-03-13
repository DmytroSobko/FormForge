using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Toast;
using UnityEngine;

namespace Infrastructure.Services.ToastService
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
            var message = new ToastShowMessage(toast, ToastType.Success);
            m_MessageService.Send(message);
        }

        public void Error(string toast)
        {
            var message = new ToastShowMessage(toast, ToastType.Error);
            m_MessageService.Send(message);
        }

        public void Warning(string toast)
        {
            var message = new ToastShowMessage(toast, ToastType.Warning);
            m_MessageService.Send(message);
        }
    }
}