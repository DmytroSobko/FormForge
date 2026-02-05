using System.Threading.Tasks;
using FormForge.Core.Services;
using UnityEngine;

namespace FormForge.Services.Initialization
{
    public class InitializationService : IInitializationService
    {
        private readonly IConfigService m_ConfigService;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IInitializationService, InitializationService>(ServiceLifespan.LazySingleton);
        }
        
        public InitializationService()
        {
            m_ConfigService = ServiceLocator.GetService<IConfigService>();
        }

        public async Task Initialize()
        {
            await m_ConfigService.LoadConfigsAsync();
        }
    }
}