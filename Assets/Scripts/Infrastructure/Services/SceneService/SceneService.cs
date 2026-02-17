using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services.Enums;
using FormForge.Infrastructure.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using ILogger = FormForge.Infrastructure.Logging.ILogger;

namespace FormForge.Infrastructure.Services.SceneService
{
	/// <inheritdoc/>
	internal class SceneService : ISceneService
	{
		public event Action<string> SceneLoaded;
		public event Action<string> SceneUnloaded;
		
		private IEnumerable<string> m_Scenes
		{
			get
			{
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					yield return SceneManager.GetSceneAt(i).name;
				}
			}
		}
		
		private Dictionary<string, AsyncOperation> m_LoadingScenes = new Dictionary<string, AsyncOperation>();
		private Dictionary<string, AsyncOperation> m_UnloadingScenes = new Dictionary<string, AsyncOperation>();
		private HashSet<string> m_LoadingScenesToUnload = new HashSet<string>();
		private HashSet<string> m_UnloadingScenesToLoad = new HashSet<string>();
		private string m_ActiveSceneName;
		private ILogger m_Logger = new UnityLogger(nameof(SceneService));
		
		[Preserve]
		public SceneService()
		{
            
		}
        
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void RegisterSelf()
		{
			ServiceLocator.RegisterService<ISceneService, SceneService>(ServiceLifespan.LazySingleton);
		}

		public async UniTask LoadScenesAsync(IEnumerable<string> sceneNames)
		{
			foreach (string sceneName in sceneNames)
			{
				await LoadSceneAsync(sceneName);
			}
		}

		public async UniTask LoadSceneAsync(string sceneName)
		{
			m_Logger?.Log($"[Load] Request load '{sceneName}'");

			// Remove from pending unload
			m_LoadingScenesToUnload.Remove(sceneName);

			if (IsSceneLoaded(sceneName))
			{
				m_Logger?.Log($"[Load] '{sceneName}' already loaded");
				return;
			}
			
			if (IsSceneUnloading(sceneName))
			{
				m_Logger?.Log($"[Load] '{sceneName}' currently unloading → postponed");
				PostponeLoadScene(sceneName);
				return;
			}

			if (IsSceneLoading(sceneName))
			{
				m_Logger?.Log($"[Load] '{sceneName}' already loading → awaiting");
				await m_LoadingScenes[sceneName].AsTask();
				return;
			}

			m_Logger?.Log($"[Load] Starting async load '{sceneName}'");
			AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			m_LoadingScenes[sceneName] = op;

			await op.AsTask();

			await OnLoadingOperationCompleted(sceneName);
		}
		
		public async UniTask UnloadScenesAsync(IEnumerable<string> sceneNames)
		{
			foreach (string sceneName in sceneNames)
			{
				await UnloadSceneAsync(sceneName);
			}
		}

		public async UniTask UnloadSceneAsync(string sceneName)
		{
			m_Logger?.Log($"[Unload] Request unload '{sceneName}'");
			
			m_UnloadingScenesToLoad.Remove(sceneName);

			if (!IsSceneLoaded(sceneName))
			{
				m_Logger?.Log($"[Unload] '{sceneName}' not loaded → skip");
				return;
			}

			if (IsSceneLoading(sceneName))
			{
				m_Logger?.Log($"[Unload] '{sceneName}' still loading → postponed");
				PostponeUnloadScene(sceneName);
				return;
			}

			if (IsSceneUnloading(sceneName))
			{
				m_Logger?.Log($"[Unload] '{sceneName}' already unloading → awaiting");
				await m_UnloadingScenes[sceneName].AsTask();
				return;
			}
			
			m_Logger?.Log($"[Unload] Starting async unload '{sceneName}'");
			
			AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);
			m_UnloadingScenes[sceneName] = op;

			await op.AsTask();

			m_UnloadingScenes.Remove(sceneName);
			SceneUnloaded?.Invoke(sceneName);
			
			m_Logger?.Log($"[Unload] Completed unload '{sceneName}'");
			await LateLoadScene(sceneName);
		}

		public void SetActiveScene(string sceneName)
		{
			m_ActiveSceneName = sceneName;
			m_Logger?.Log($"[Active] Set active scene '{sceneName}'");
			
			if (IsSceneLoaded(sceneName))
			{
				Scene scene = SceneManager.GetSceneByName(sceneName);
				SceneManager.SetActiveScene(scene);
			}
			else
			{
				m_Logger?.LogWarning($"[Active] '{sceneName}' not loaded yet");
			}
		}

		private bool IsSceneLoaded(string sceneName)
		{
			return m_Scenes.Contains(sceneName);
		}

		private bool IsSceneLoading(string sceneName)
		{
			return m_LoadingScenes.ContainsKey(sceneName);
		}

		private bool IsSceneUnloading(string sceneName)
		{
			return m_UnloadingScenes.ContainsKey(sceneName);
		}

		private void PostponeUnloadScene(string sceneName)
		{
			if (m_LoadingScenesToUnload.Add(sceneName))
			{
				m_Logger?.Log($"Postponed unload '{sceneName}'");
			}
		}

		private void PostponeLoadScene(string sceneName)
		{
			if (m_UnloadingScenesToLoad.Add(sceneName))
			{
				m_Logger?.Log($"Postponed load '{sceneName}'");
			}
		}

		private async UniTask OnLoadingOperationCompleted(string sceneName)
		{
			m_LoadingScenes.Remove(sceneName);

			m_Logger?.Log($"[Load] Completed load '{sceneName}'");

			if (await LateUnloadScene(sceneName))
			{
				return;
			}

			SceneLoaded?.Invoke(sceneName);

			if (m_ActiveSceneName == sceneName)
			{
				Scene scene = SceneManager.GetSceneByName(sceneName);
				SceneManager.SetActiveScene(scene);
			}
		}

		private async UniTask<bool> LateUnloadScene(string sceneName)
		{
			// Check whether the newly loaded scene must be unloaded.
			if (!m_LoadingScenesToUnload.Remove(sceneName))
			{
				return false;
			}
			m_Logger?.Log($"Late unload triggered for '{sceneName}'");
			await UnloadSceneAsync(sceneName);

			return true;
		}

		private async UniTask LateLoadScene(string sceneName)
		{
			// Check whether the newly unloaded scene must be loaded.
			if (m_UnloadingScenesToLoad.Remove(sceneName))
			{
				m_Logger?.Log($"Late load triggered for '{sceneName}'");
				await LoadSceneAsync(sceneName);
			}
		}
	}
}
