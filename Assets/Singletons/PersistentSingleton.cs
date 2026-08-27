using UnityEngine;

namespace LouisAshton.Singletons
{
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        public bool AutoUnparentOnAwake = true;
        
        protected static T instance;
        
        public static bool HasInstance => instance;
        public static T TryGetInstance() => HasInstance ? instance : null;

        public static T Instance {
            get {
                if (!instance) {
                    instance = FindAnyObjectByType<T>();
                    if (!instance) {
                        var go = new GameObject(typeof(T).Name + " (runtime generated)");
                        instance = go.AddComponent<T>();
                    }
                }
                
                return instance;
            }
        }

        protected virtual void Awake()
        {
            InitialiseSingleton();
        }

        protected virtual void InitialiseSingleton()
        {
            if (!Application.isPlaying)
                return;

            if(AutoUnparentOnAwake)
                transform.SetParent(null);

            if (!instance) {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this) {
                   Destroy(gameObject);
            }
        }
    }
    
}
