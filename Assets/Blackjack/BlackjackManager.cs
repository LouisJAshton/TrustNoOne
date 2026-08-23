using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class BlackjackManager
{
    public const int MAX = 21;
    
    [SerializeField] private List<CardInfo> hand;
    [SerializeField] private List<CardInfo> deck;
    
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
        hand.Add(deck[0]);
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
