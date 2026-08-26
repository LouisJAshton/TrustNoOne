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
    [SerializeField] public DeckBase deck;
    
    public Character character;
    
    private List<CardInfo> _hand = new();
    public ITurnStrategy turnStrategy;
    
    public ITutorStrategy tutorStrategy;

    private bool _isStanding = false;

    public bool IsShielded {
        get => _isShielded;
        set {
            _isShielded = value;
            OnShieldedUpdated.Invoke(value);
        }
    }

    private bool _isShielded = false;

    public bool IsStanding {
        get => _isStanding;
        set {
            _isStanding = value;
            OnStandingUpdated.Invoke(value);
        }
    }

    public List<CardInfo> Hand => _hand;
    
    public UnityEvent<List<CardInfo>> OnHandUpdated;
    public UnityEvent<bool> OnStandingUpdated;
    public UnityEvent<bool> OnShieldedUpdated;
    
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
        IsShielded = false;
        IsStanding = false;
        _hand.Clear();
        OnHandUpdated?.Invoke(_hand);
    }

    public enum Character
    {
        Player,
        Enemy,
        Dealer
    }
}
