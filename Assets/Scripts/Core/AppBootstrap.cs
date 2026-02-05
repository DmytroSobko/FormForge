using System.Threading.Tasks;
using FormForge.Core.Services;
using FormForge.Services.Initialization;
using UnityEngine;

namespace FormForge.Core
{
    public class AppBootstrap : MonoBehaviour
    {
        private IInitializationService m_InitializationService;
        
        private async void Awake()
        {
            m_InitializationService = ServiceLocator.GetService<IInitializationService>();
            
            await InitializeAsync();
        }
        
        private async Task InitializeAsync()
        {
            Debug.Log("Bootstrap started");

            await m_InitializationService.Initialize();

            Debug.Log("Bootstrap finished");
        }
    }
}