using Cysharp.Threading.Tasks;
using FormForge.AssetManagement.Addressable.AssetLoader;
using FormForge.AssetManagement.CacheStrategy;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.HttpClientService;
using FormForge.Infrastructure.Services.MessageService.Interfaces;
using FormForge.Infrastructure.UI.Overlays.LoadingOverlay.Messages;
using FormForge.Networking;
using FormForge.Services.ConfigsService;
using FormForge.Services.VisualsService;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.InitializationService
{
    public class InitializationService : IInitializationService
    {
        private readonly IHttpClientService m_HttpClient;
        private readonly IConfigsService m_ConfigService;
        private readonly IMessageService m_MessageService;
        private readonly IVisualsService m_VisualsService;

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
            m_VisualsService = ServiceLocator.GetService<IVisualsService>();
        }

        public async UniTask Initialize()
        {
            m_Logger?.Log("Starting Initialize");

            m_MessageService.Send(new LoadingOverlayShowMessage());
            
            IAddressableAssetLoader assetLoader = new AddressableAssetLoader();
            await assetLoader.InitializeAsync();
            
            m_MessageService.Send(new LoadingOverlaySetProgressMessage(0.25f));

            IAssetManagementService assetManagementService = 
                new AssetManagementService(assetLoader, new DynamicCache(), new NoCache());
            ServiceLocator.RegisterSingletonService(assetManagementService);

            m_MessageService.Send(new LoadingOverlaySetProgressMessage(0.35f));
            m_HttpClient.SetBaseApiUrl(APIConfig.BaseUrl);
            
            m_MessageService.Send(new LoadingOverlaySetProgressMessage(0.5f));
            
            await m_VisualsService.InitializeAsync();
            
            m_MessageService.Send(new LoadingOverlaySetProgressMessage(0.65f));

            await m_ConfigService.LoadConfigsAsync();

            m_MessageService.Send(new LoadingOverlaySetProgressMessage(0.75f));
            
            m_Logger?.Log("Starting Ended");
        }
    }
}