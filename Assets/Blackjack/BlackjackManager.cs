using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BlackjackManager
{
    public const int MAX = 21;

    [SerializeField] private List<BaseCardInfo> baseDeck;
    
    [SerializeField] private List<CardInfo> hand;
    [SerializeField] private List<CardInfo> deck;
    
    public UnityEvent<CardInfo> OnDraw;

    public void Initialise()
    {
        hand = new List<CardInfo>();
        deck = new List<CardInfo>();

        foreach (var card in baseDeck) {
            deck.Add(card.BaseInfo);
        }
    }
    
    public int CalculateScore(PlayerData playerData)
    {
        var total = 0;
        var acesCount = 0;
        
        var totalHand = new List<CardInfo>();
        totalHand.AddRange(hand);
        totalHand.AddRange(playerData.Hand);

        foreach (var card in totalHand) {
            
            //Handle non-aces
            if (card.rank != 1) {
                total += card.rank;
            }
            //Handle aces
            else {
                acesCount++;
            }
        }

        for (var i = acesCount; i >= 0; i--) {
            var acesValue = i * 11;
            
            if (acesValue + total <= 21) {
                total += acesValue;
                break;
            }
            else {
                total += 1;
            }
        }
        
        return total;
    }

    public void Draw()
    {
        if (deck == null || deck.Count == 0) {
            Debug.Log("Drawing from empty deck");
            return;
        }
        
        //TODO Replace with actual draw
        var card = deck[0];
        
        hand.Add(card);
        OnDraw.Invoke(card);
        
        deck.RemoveAt(0);
    }

    public void DebugDealerHand()
    {
        StringBuilder sb = new();
        foreach (var card in hand) {
            sb.Append(card.rankName);
            sb.Append(" of ");
            sb.Append(card.suit);
            sb.Append("s | ");
        }
    }
}
