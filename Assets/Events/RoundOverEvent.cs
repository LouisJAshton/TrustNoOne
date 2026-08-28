using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "RoundOverEvent", menuName = "Scriptable Objects/RoundOverEvent")]
public class RoundOverEvent : ScriptableObject
{
    private UnityEvent<RoundOverEventData> onRoundOverEvent = new UnityEvent<RoundOverEventData>();

    public void Trigger(RoundOverEventData eventData)
    {
        onRoundOverEvent.Invoke(eventData);
    }
    
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
    public readonly DialogueHandler.CharacterName CharacterName;
    public readonly bool WasWon;
    public RoundOverEventData(DialogueHandler.CharacterName characterName, bool wasWon)
    {
        this.CharacterName = characterName;
        WasWon = wasWon;
    }
}
