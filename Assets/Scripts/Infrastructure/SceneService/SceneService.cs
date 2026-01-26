using System;
using System.Collections.Generic;
using System.Linq;
using FormForge.Core.Services;
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
		
		public void LoadScenes(IEnumerable<string> sceneNames)
		{
			foreach (string sceneName in sceneNames)
			{
				LoadScene(sceneName);
			}
		}

		public void LoadScene(string sceneName)
		{
			// Remove it from pending to unload since it is requested to load it again.
			m_loadingScenesToUnload.Remove(sceneName);

			if (!IsSceneLoaded(sceneName) && !IsSceneLoading(sceneName))
			{
				LoadSceneAsync(sceneName);
			}
			else if (IsSceneUnloading(sceneName))
			{
				PostponeLoadScene(sceneName);
			}
		}
		
		public void UnloadScenes(IEnumerable<string> sceneNames)
		{
			foreach (string sceneName in sceneNames)
			{
				UnloadScene(sceneName);
			}
		}

		public void UnloadScene(string sceneName)
		{
			// Remove it from pending to load since it is requested to unload it again.
			m_unloadingScenesToLoad.Remove(sceneName);

			if (IsSceneLoaded(sceneName) && !IsSceneUnloading(sceneName))
			{
				UnloadSceneAsync(sceneName);
			}
			else if (m_loadingScenes.ContainsKey(sceneName))
			{
				PostponeUnloadScene(sceneName);
			}
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
		
	    private void LoadSceneAsync(string sceneName)
		{
			if (string.IsNullOrEmpty(sceneName))
			{
				return;
			}
			
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			
			m_loadingScenes.Add(sceneName, asyncOperation);
			asyncOperation.completed += _ =>
			{
				OnLoadingOperationCompleted(sceneName);
			};
		}

		private void UnloadSceneAsync(string sceneName)
		{
			if (string.IsNullOrEmpty(sceneName))
			{
				return;
			}
			
			AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);

			m_unloadingScenes[sceneName] = asyncOperation;
			asyncOperation.completed += _ =>
			{
				m_unloadingScenes.Remove(sceneName);
				SceneUnloaded?.Invoke(sceneName);
				LateLoadScene(sceneName);
			};
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

		private void OnLoadingOperationCompleted(string sceneName)
		{
			m_loadingScenes.Remove(sceneName);

			// Check whether the newly loaded scene must be unloaded.
			if (LateUnloadScene(sceneName))
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

		private bool LateUnloadScene(string sceneName)
		{
			// Check whether the newly loaded scene must be unloaded.
			if (m_loadingScenesToUnload.Remove(sceneName))
			{
				UnloadScene(sceneName);

				return true;
			}

			return false;
		}

		private bool LateLoadScene(string sceneName)
		{
			// Check whether the newly unloaded scene must be loaded.
			if (m_unloadingScenesToLoad.Remove(sceneName))
			{
				LoadScene(sceneName);

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
