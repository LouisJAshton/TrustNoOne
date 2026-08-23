using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerData
{
    [SerializeField] public string playerName;
    
    private List<CardInfo> _hand = new();
    public ITurnStrategy turnStrategy;

    public bool isStanding = false;
    
    public List<CardInfo> Hand => _hand;
    
    public UnityEvent<List<CardInfo>> OnHandUpdated;
    
    public void AddCards(params CardInfo[] cards)
    {
        _hand.AddRange(cards);
        OnHandUpdated?.Invoke(_hand);
    }

    public void RemoveCards(params CardInfo[] cards)
    {
        _hand.RemoveAll(cards.Contains);
        OnHandUpdated?.Invoke(_hand);
    }

    public void Reset()
    {
        isStanding = false;
        _hand.Clear();
        OnHandUpdated?.Invoke(_hand);
    }
    
}
