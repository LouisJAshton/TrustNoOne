using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Combat.UI
{
    public class LogManager : MonoBehaviour
    {
        public static LogManager Instance { get; private set; }
        [SerializeField] private OnScreenMessage onScreenMessagePrefab;
        [SerializeField] private List<LogData> logs = new();

        private readonly IMessageFactory _messageFactory = new MessageFactory();
        
        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(transform.parent.gameObject);
        }
        
        public void Log(LogData data)
        {
            var log = _messageFactory.Create(data, onScreenMessagePrefab);
            log.gameObject.transform.SetParent(transform, false);
            logs.Add(data);
        }
    }

    internal interface IMessageFactory
    {
        OnScreenMessage Create(LogData data, OnScreenMessage prefab);
    }

    internal class MessageFactory : IMessageFactory
    {
        public OnScreenMessage Create(LogData data, OnScreenMessage prefab)
        {
            var message = Object.Instantiate(prefab);
            message.Set(data);
            return message;
        }
    }
}
