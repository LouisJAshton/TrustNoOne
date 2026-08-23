using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerData
{
    [SerializeField] private List<CardInfo> hand;
    
    public List<CardInfo> Hand => hand;
    
    public UnityEvent<List<CardInfo>> OnHandUpdated;

    public void AddCards(params CardInfo[] cards)
    {
        hand.AddRange(cards);
        OnHandUpdated?.Invoke(hand);
    }

    public void RemoveCards(params CardInfo[] cards)
    {
        hand.RemoveAll(cards.Contains);
        OnHandUpdated?.Invoke(hand);
    }

    public void Clear()
    {
        hand.Clear();
        OnHandUpdated?.Invoke(hand);
    }
    
}
