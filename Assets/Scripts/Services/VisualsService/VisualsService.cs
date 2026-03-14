using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Domain.Athletes;
using FormForge.Domain.Exercises;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.UI.Toast;
using FormForge.ScriptableObjects.Visuals.Athletes;
using FormForge.ScriptableObjects.Visuals.Exercises;
using FormForge.ScriptableObjects.Visuals.Toasts;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.VisualsService
{
    public class VisualsService : IVisualsService
    {
        private AthleteTypeVisualsDatabase m_AthleteTypeVisualsDatabase;
        private ExerciseVisualsDatabase m_ExerciseVisualsDatabase;
        private ToastVisualsDatabase m_ToastVisualsDatabase;

        private readonly ILogger m_Logger = new UnityLogger(nameof(VisualsService));

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IVisualsService, VisualsService>(ServiceLifespan.LazySingleton);
        }

        public async UniTask InitializeAsync()
        {
            m_Logger?.Log("InitializeAsync Started");

            var athleteTask = LoadDatabaseAsync<AthleteTypeVisualsDatabase>(
                AddressKeys.ScriptableObjects.VisualDatabases.AthleteTypeVisualsDatabase);

            var exerciseTask = LoadDatabaseAsync<ExerciseVisualsDatabase>(
                AddressKeys.ScriptableObjects.VisualDatabases.ExerciseVisualsDatabase);

            var toastTask = LoadDatabaseAsync<ToastVisualsDatabase>(
                AddressKeys.ScriptableObjects.VisualDatabases.ToastVisualsDatabase);
            
            (m_AthleteTypeVisualsDatabase, m_ExerciseVisualsDatabase, m_ToastVisualsDatabase) = 
                await UniTask.WhenAll(athleteTask, exerciseTask, toastTask);

            m_AthleteTypeVisualsDatabase.Initialize();
            m_ExerciseVisualsDatabase.Initialize();
            m_ToastVisualsDatabase.Initialize();

            m_Logger?.Log("InitializeAsync Ended");
        }

        private async UniTask<TDatabase> LoadDatabaseAsync<TDatabase>(string addressKey)
            where TDatabase : ScriptableObject
        {
            IAssetManagementService assetManagementService = ServiceLocator.GetService<IAssetManagementService>();
            var policy = new BasicAssetPolicy(addressKey);
            return await assetManagementService.LoadAsync<TDatabase, UIContext>(policy);
        }

        public AthleteTypeVisualsConfig GetAthleteTypeVisuals(EAthleteType type)
        {
            return m_AthleteTypeVisualsDatabase.Get(type);
        }

        public ExerciseVisualsConfig GetExerciseVisuals(EExerciseType type)
        {
            return m_ExerciseVisualsDatabase.Get(type);
        }
        
        public ToastVisualsConfig GetToastVisuals(EToastType type)
        {
            return m_ToastVisualsDatabase.Get(type);
        }
    }
}