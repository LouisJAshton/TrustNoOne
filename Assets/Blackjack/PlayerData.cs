using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerData
{
    [SerializeField] public string playerName;
    [SerializeField] public DeckBase baseDeck;
    
    [NonSerialized] public List<CardInfo> deck;
    
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
    
    //public UnityEvent<List<CardInfo>> OnHandUpdated;

    public HandDisplayManager handDisplayManagers;
    
    public UnityEvent<bool> OnStandingUpdated;
    public UnityEvent<bool> OnShieldedUpdated;
    
    public async UniTask AddCards(params CardInfo[] cards)
    {
        _hand.AddRange(cards);

        await UpdateHand();
    }

    public async UniTask RemoveCards(params CardInfo[] cards)
    {
        _hand.RemoveAll(cards.Contains);
    
        await UpdateHand();
    }

    public async UniTask Reset()
    {
        IsShielded = false;
        IsStanding = false;

        deck = baseDeck.GetDeck();
        
        _hand.Clear();
        
        await UpdateHand();
    }

    private async UniTask UpdateHand()
    {
        await handDisplayManagers.UpdateHand(_hand);
        
        Debug.Log("Updated");
    }

    public enum Character
    {
        Player,
        Enemy,
        Dealer
    }
}
