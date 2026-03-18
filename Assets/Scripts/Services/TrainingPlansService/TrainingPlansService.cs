using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormForge.Domain.TrainingPlans;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services;
using FormForge.Infrastructure.Services.CacheService;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Services.HttpClientService;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Services.TrainingPlansService
{
    public class TrainingPlansService : ITrainingPlansService
    {
        private const int k_PageSize = 100;
        private const string k_TrainingPlansCacheKey = "training_plans";
        private static readonly TimeSpan s_CacheLifetime = TimeSpan.FromMinutes(5);
        
        private ILogger m_Logger = new UnityLogger(nameof(TrainingPlansService));

        private readonly ICacheService m_CacheService;
        private readonly IHttpClientService m_HttpClientService;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<ITrainingPlansService, TrainingPlansService>(ServiceLifespan.LazySingleton);
        }
        
        public UniTask<TrainingPlan> CreateTrainingPlan(string name)
        {
            throw new System.NotImplementedException();
        }

        public UniTask<IReadOnlyList<TrainingPlan>> GetTrainingPlans()
        {
            return UniTask.FromResult<IReadOnlyList<TrainingPlan>>(Array.Empty<TrainingPlan>());
        }
    }
}