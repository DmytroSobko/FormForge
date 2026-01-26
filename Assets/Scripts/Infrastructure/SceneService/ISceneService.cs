using System;
using System.Collections.Generic;
using UnityEngine;

namespace FormForge.SceneService
{
    /// <summary>
    /// Service to handle the loading and unloading of scenes
    /// </summary>
    public interface ISceneService
    {
        /// <summary>
        /// Event fired when a scene has finished loading. Sends the name of the scene that has finished loading
        /// </summary>
        public event Action<string> SceneLoaded;
        
        /// <summary>
        /// Event fired when a scene has finished unloading. Sends the name of the scene that has finished unloading
        /// </summary>
        public event Action<string> SceneUnloaded;

        /// <summary>
        /// Defines custom logging functionality for the SceneService to use. If not set, the ViewManager will output through Unity's Debug class.
        /// </summary>
        /// <param name="logger"></param>
        public void SetLogger(ILogger logger);
        
        /// <summary>
        /// Start loading a set of scenes
        /// </summary>
        /// <param name="sceneNames">Scenes to load</param>
        public void LoadScenes(IEnumerable<string> sceneNames);
        
        /// <summary>
        /// Start loading a single scene
        /// </summary>
        /// <param name="sceneName">Scene to load</param>
        public void LoadScene(string sceneName);
        
        /// <summary>
        /// Start unloading a set of scenes
        /// </summary>
        /// <param name="sceneNames">Scenes to unload</param>
        public void UnloadScenes(IEnumerable<string> sceneNames);
        
        /// <summary>
        /// Start unloading a single scene
        /// </summary>
        /// <param name="sceneName">Scene to unload</param>
        public void UnloadScene(string sceneName);
        
        /// <summary>
        /// Set a scene as the current active scene
        /// </summary>
        /// <param name="sceneName">Scene to set as active</param>
        public void SetActiveScene(string sceneName);
    }
}
