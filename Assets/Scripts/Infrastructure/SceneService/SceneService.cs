using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormForge.Core.Services;
using FormForge.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace FormForge.SceneService
{
	/// <inheritdoc/>
	internal class SceneService : ISceneService
	{
		public event Action<string> SceneLoaded;
		public event Action<string> SceneUnloaded;
		
		private IEnumerable<string> m_scenes
		{
			get
			{
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					yield return SceneManager.GetSceneAt(i).name;
				}
			}
		}
		
		private Dictionary<string, AsyncOperation> m_loadingScenes = new Dictionary<string, AsyncOperation>();
		private Dictionary<string, AsyncOperation> m_unloadingScenes = new Dictionary<string, AsyncOperation>();
		private HashSet<string> m_loadingScenesToUnload = new HashSet<string>();
		private HashSet<string> m_unloadingScenesToLoad = new HashSet<string>();
		private string m_activeSceneName;
		private ILogger m_logger;
		
		[Preserve]
		public SceneService()
		{
            
		}
        
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void RegisterSelf()
		{
			ServiceLocator.RegisterService<ISceneService, SceneService>(ServiceLifespan.LazySingleton);
		}
		
		public void SetLogger(ILogger logger)
		{
			m_logger = logger;
		}
		
		public async Task LoadScenesAsync(IEnumerable<string> sceneNames)
		{
			foreach (string sceneName in sceneNames)
			{
				await LoadSceneAsync(sceneName);
			}
		}

		public async Task LoadSceneAsync(string sceneName)
		{
			// Remove from pending unload
			m_loadingScenesToUnload.Remove(sceneName);

			if (IsSceneLoaded(sceneName))
				return;

			if (IsSceneUnloading(sceneName))
			{
				PostponeLoadScene(sceneName);
				return;
			}

			if (IsSceneLoading(sceneName))
			{
				await m_loadingScenes[sceneName].AsTask();
				return;
			}

			AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			m_loadingScenes[sceneName] = op;

			await op.AsTask();

			await OnLoadingOperationCompleted(sceneName);
		}
		
		public async Task UnloadScenesAsync(IEnumerable<string> sceneNames)
		{
			foreach (string sceneName in sceneNames)
			{
				await UnloadSceneAsync(sceneName);
			}
		}

		public async Task UnloadSceneAsync(string sceneName)
		{
			m_unloadingScenesToLoad.Remove(sceneName);

			if (!IsSceneLoaded(sceneName))
				return;

			if (IsSceneLoading(sceneName))
			{
				PostponeUnloadScene(sceneName);
				return;
			}

			if (IsSceneUnloading(sceneName))
			{
				await m_unloadingScenes[sceneName].AsTask();
				return;
			}

			AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);
			m_unloadingScenes[sceneName] = op;

			await op.AsTask();

			m_unloadingScenes.Remove(sceneName);
			SceneUnloaded?.Invoke(sceneName);
			LateLoadScene(sceneName);
		}

		public void SetActiveScene(string sceneName)
		{
			m_activeSceneName = sceneName;

			if (IsSceneLoaded(sceneName))
			{
				Scene scene = SceneManager.GetSceneByName(sceneName);
				SceneManager.SetActiveScene(scene);
			}
		}

		private bool IsSceneLoaded(string sceneName)
		{
			return m_scenes.Contains(sceneName);
		}

		private bool IsSceneLoading(string sceneName)
		{
			return m_loadingScenes.ContainsKey(sceneName);
		}

		private bool IsSceneUnloading(string sceneName)
		{
			return m_unloadingScenes.ContainsKey(sceneName);
		}

		private void PostponeUnloadScene(string sceneName)
		{
			if (m_loadingScenesToUnload.Add(sceneName))
			{
				Log($"Trying to unload a not ready/loaded yet scene : {sceneName}");
			}
		}

		private void PostponeLoadScene(string sceneName)
		{
			if (m_unloadingScenesToLoad.Add(sceneName))
			{
				Log($"Trying to load a not ready/unloaded yet scene : {sceneName}");
			}
		}

		private async Task OnLoadingOperationCompleted(string sceneName)
		{
			m_loadingScenes.Remove(sceneName);

			// Check whether the newly loaded scene must be unloaded.

			var res = await LateUnloadScene(sceneName);
			if (res)
			{
				return;
			}

			SceneLoaded?.Invoke(sceneName);

			if (m_activeSceneName == sceneName)
			{
				Scene scene = SceneManager.GetSceneByName(sceneName);
				SceneManager.SetActiveScene(scene);
			}
		}

		private async Task<bool> LateUnloadScene(string sceneName)
		{
			// Check whether the newly loaded scene must be unloaded.
			if (m_loadingScenesToUnload.Remove(sceneName))
			{
				await UnloadSceneAsync(sceneName);

				return true;
			}

			return false;
		}

		private async Task<bool> LateLoadScene(string sceneName)
		{
			// Check whether the newly unloaded scene must be loaded.
			if (m_unloadingScenesToLoad.Remove(sceneName))
			{
				await LoadSceneAsync(sceneName);

				return true;
			}

			return false;
		}
		
		private void Log(string msg)
		{
			if (m_logger == null)
			{
				m_logger = Debug.unityLogger;
			}
            
			m_logger.Log(nameof(SceneService), msg);
		}

		private void LogWarning(string msg)
		{
			if (m_logger == null)
			{
				m_logger = Debug.unityLogger;
			}
            
			m_logger.LogWarning(nameof(SceneService), msg);
		}

		private void LogError(string msg)
		{
			if (m_logger == null)
			{
				m_logger = Debug.unityLogger;
			}
            
			m_logger.LogError(nameof(SceneService), msg);
		}
	}
}
