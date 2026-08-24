using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

[Serializable]
public class PlayerData
{
    [SerializeField] public string playerName;
    
    private List<CardInfo> _hand = new();
    public ITurnStrategy turnStrategy;
    
    public ITutorStrategy tutorStrategy;

    private bool isStanding = false;

    public bool IsStanding {
        get => isStanding;
        set {
            isStanding = value;
            OnStandingUpdated.Invoke(value);
        }
    }

    public List<CardInfo> Hand => _hand;
    
    public UnityEvent<List<CardInfo>> OnHandUpdated;
    public UnityEvent<bool> OnStandingUpdated;
    
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
        IsStanding = false;
        _hand.Clear();
        OnHandUpdated?.Invoke(_hand);
    }
    
}
