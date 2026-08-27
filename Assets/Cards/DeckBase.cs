using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckBase", menuName = "Scriptable Objects/DeckBase")]
public class DeckBase : ScriptableObject
{
    [SerializeField] private List<CardTypeAmount> deck;
    //TODO Customise card back here?
    
    public List<CardInfo> GetDeck()
    {
        List<CardInfo> output = new();

        foreach (CardTypeAmount c in deck) {
            for (int i = 0; i < c.amount; i++) {
                output.Add(c.cardType._cardInfo.Clone());
            }
        }
        
        return output;
    }
}

[Serializable]
public struct CardTypeAmount
{
    public BaseCardInfo cardType;
    [Min(1)] public int amount;
}