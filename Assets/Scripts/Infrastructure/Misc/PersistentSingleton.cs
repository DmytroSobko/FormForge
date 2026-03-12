using UnityEngine;

namespace FormForge.Infrastructure.Misc
{
    /// <summary>
    /// Generic persistent singleton base.
    /// Ensures exactly one instance exists and survives scene loads.
    /// </summary>
    public abstract class PersistentSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T m_Instance;
        private static bool m_IsQuitting;

        public static T Instance
        {
            get
            {
                if (m_IsQuitting)
                {
                    Debug.LogWarning($"[{typeof(T).Name}] Instance requested while application is quitting.");
                    return null;
                }

                if (m_Instance != null)
                {
                    return m_Instance;
                }
                m_Instance = FindFirstObjectByType<T>();

                if (m_Instance != null)
                {
                    return m_Instance;
                }
                var go = new GameObject(typeof(T).Name);
                m_Instance = go.AddComponent<T>();

                return m_Instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_Instance != null && m_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            m_Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnApplicationQuit()
        {
            m_IsQuitting = true;
        }
    }
}