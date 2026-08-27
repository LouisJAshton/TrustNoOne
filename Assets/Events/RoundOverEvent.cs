using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "RoundOverEvent", menuName = "Scriptable Objects/RoundOverEvent")]
public class RoundOverEvent : ScriptableObject
{
    private UnityEvent<RoundOverEventData> onRoundOverEvent = new UnityEvent<RoundOverEventData>();

    public void Subscribe(UnityAction<RoundOverEventData> action)
    {
        onRoundOverEvent.RemoveListener(action);
        onRoundOverEvent.AddListener(action);
    }
    
    public void Unsubscribe(UnityAction<RoundOverEventData> action)
    {
        onRoundOverEvent.RemoveListener(action);
    }

    public void Clear()
    {
        onRoundOverEvent.RemoveAllListeners();
    }
}

public struct RoundOverEventData
{
    public EnemySetupData enemyData;
    public bool wasWon;
}
