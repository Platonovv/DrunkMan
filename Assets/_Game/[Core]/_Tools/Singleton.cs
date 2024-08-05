using UnityEngine;

namespace _Tools
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!IsInitialized)
                {
                    _instance = FindObjectOfType<T>(true);
                }

                return _instance;
            }
        }

        public static bool IsInitialized => _instance != null;

        protected virtual void Awake()
        {
            if (IsInitialized && Instance != this)
            {
                Debug.LogWarning("[Singleton] Trying to instantiate a second instance of a singleton class.");
            }
            else
            {
                _instance = (T)this;
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                _instance = null;
            }
        }
    }
}