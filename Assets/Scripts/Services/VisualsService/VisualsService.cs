using Cysharp.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Domain.Athletes;
using FormForge.Domain.Exercises;
using FormForge.Infrastructure.AssetManagementService;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.Enums;
using FormForge.ScriptableObjects.Athletes;
using FormForge.ScriptableObjects.Exercises;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.VisualsService
{
    public class VisualsService : IVisualsService
    {
        private AthleteTypeVisualsDatabase m_AthleteTypeVisualsDatabase;
        private ExerciseVisualsDatabase m_ExerciseVisualsDatabase;

        private readonly ILogger m_Logger = new UnityLogger(nameof(VisualsService));
        private readonly IAssetManagementService m_AssetManagementService;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IVisualsService, VisualsService>(ServiceLifespan.LazySingleton);
        }

        public VisualsService()
        {
            m_AssetManagementService = ServiceLocator.GetService<IAssetManagementService>();
        }

        public async UniTask InitializeAsync()
        {
            m_Logger?.Log("InitializeAsync Started");

            var athleteTask = LoadDatabaseAsync<AthleteTypeVisualsDatabase>(
                AddressKeys.ScriptableObjects.VisualDatabases.AthleteTypeVisualsDatabase);

            var exerciseTask = LoadDatabaseAsync<ExerciseVisualsDatabase>(
                AddressKeys.ScriptableObjects.VisualDatabases.ExerciseVisualsDatabase);

            await UniTask.WhenAll(athleteTask, exerciseTask);

            m_AthleteTypeVisualsDatabase = await athleteTask;
            m_ExerciseVisualsDatabase = await exerciseTask;

            m_AthleteTypeVisualsDatabase.Initialize();
            m_ExerciseVisualsDatabase.Initialize();

            m_Logger?.Log("InitializeAsync Ended");
        }

        private async UniTask<TDatabase> LoadDatabaseAsync<TDatabase>(string addressKey)
            where TDatabase : ScriptableObject
        {
            var policy = new BasicAssetPolicy(addressKey);
            return await m_AssetManagementService.LoadAsync<TDatabase, UIContext>(policy);
        }

        public AthleteTypeVisualsConfig GetAthleteTypeVisuals(EAthleteType type)
        {
            return m_AthleteTypeVisualsDatabase.Get(type);
        }

        public ExerciseVisualsConfig GetExerciseVisuals(EExerciseType type)
        {
            return m_ExerciseVisualsDatabase.Get(type);
        }
    }
}