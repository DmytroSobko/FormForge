using System.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.Addressable.AssetLoader;
using FormForge.AssetManagement.CacheStrategy;
using FormForge.Core.Config;
using FormForge.Core.Services;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Networking;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.Initialization
{
    public class InitializationService : IInitializationService
    {
        private readonly IHttpClientService m_HttpClient;
        private readonly IConfigService m_ConfigService;
        
        private ILogger m_Logger = new UnityLogger(nameof(InitializationService));
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IInitializationService, InitializationService>(ServiceLifespan.LazySingleton);
        }
        
        public InitializationService()
        {
            m_HttpClient = ServiceLocator.GetService<IHttpClientService>();
            m_ConfigService = ServiceLocator.GetService<IConfigService>();
        }

        public async Task Initialize()
        {
            m_Logger?.Log("AssetManagementService Initialization");

            IAddressableAssetLoader assetLoader = new AddressableAssetLoader();
            await assetLoader.InitializeAsync();
            
            IAssetManagementService assetManagementService = 
                new AssetManagementService(assetLoader, new DynamicCache(), new NoCache());
            ServiceLocator.RegisterSingletonService(assetManagementService);

            m_Logger?.Log($"SetBaseApiUrl {EnvironmentConfig.ApiBaseUrl}");

            m_HttpClient.SetBaseApiUrl(EnvironmentConfig.ApiBaseUrl);
            
            m_Logger?.Log("LoadConfigsAsync Started");
            
            await m_ConfigService.LoadConfigsAsync();
            
            m_Logger?.Log("LoadConfigsAsync Ended");
        }
    }
}