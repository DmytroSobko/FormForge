using System.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.Addressable.AssetLoader;
using FormForge.AssetManagement.CacheStrategy;
using FormForge.Core;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.HttpClientService;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.LoadingOverlay.Messages;
using FormForge.Services.ConfigsService;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.InitializationService
{
    public class InitializationService : IInitializationService
    {
        private readonly IHttpClientService m_HttpClient;
        private readonly IConfigsService m_ConfigService;
        private readonly IMessageService m_MessageService;

        private ILogger m_Logger = new UnityLogger(nameof(InitializationService));
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IInitializationService, InitializationService>(ServiceLifespan.LazySingleton);
        }
        
        public InitializationService()
        {
            m_HttpClient = ServiceLocator.GetService<IHttpClientService>();
            m_ConfigService = ServiceLocator.GetService<IConfigsService>();
            m_MessageService = ServiceLocator.GetService<IMessageService>();
        }

        public async Task Initialize()
        {
            m_MessageService.Send(new LoadingOverlayShowMessage());
            
            m_Logger?.Log("AssetManagementService Initialization");

            IAddressableAssetLoader assetLoader = new AddressableAssetLoader();
            await assetLoader.InitializeAsync();
            
            m_MessageService.Send(new LoadingOverlaySetProgressMessage(0.25f));

            IAssetManagementService assetManagementService = 
                new AssetManagementService(assetLoader, new DynamicCache(), new NoCache());
            ServiceLocator.RegisterSingletonService(assetManagementService);

            m_MessageService.Send(new LoadingOverlaySetProgressMessage(0.35f));

            m_Logger?.Log($"SetBaseApiUrl {EnvironmentConfig.ApiBaseUrl}");

            m_HttpClient.SetBaseApiUrl(EnvironmentConfig.ApiBaseUrl);
            
            m_MessageService.Send(new LoadingOverlaySetProgressMessage(0.5f));

            m_Logger?.Log("LoadConfigsAsync Started");
            
            await m_ConfigService.LoadConfigsAsync();
            
            m_Logger?.Log("LoadConfigsAsync Ended");
            
            m_MessageService.Send(new LoadingOverlaySetProgressMessage(0.75f));
        }
    }
}