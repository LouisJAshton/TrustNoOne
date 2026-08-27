using UnityEngine;

namespace LouisAshton.Singletons
{
    public class RegulatorSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance;
        
        public static bool HasInstance => instance;

        public float InitialisationTime { get; private set; }
        
        public static T Instance {
            get {
                if (!instance) {
                    instance = FindAnyObjectByType<T>();
                    if (!instance) {
                        var go = new GameObject(typeof(T).Name + " (runtime generated)");
                        go.hideFlags = HideFlags.HideAndDontSave;
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
            
            InitialisationTime = Time.time;
            DontDestroyOnLoad(gameObject);

            T[] oldInstances = FindObjectsByType<T>();
            foreach (T old in oldInstances) {
                if(old.GetComponent<RegulatorSingleton<T>>().InitialisationTime < InitialisationTime)
                {
                    Destroy(old.gameObject);
                }
            }
            
            if (!instance) {
                instance = this as T;
            }
        }
    }
}

