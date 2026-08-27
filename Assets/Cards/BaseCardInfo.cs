using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class BaseCardInfo : ScriptableObject
{
    public CardInfo BaseInfo => _cardInfo.Clone();
    
    [FormerlySerializedAs("baseCardInfo")] public CardInfo _cardInfo;
    
}

[Serializable]
public class CardInfo
{
    public CardInfo Clone()
    {
        var output = new CardInfo();
        output.rank = rank;
        output.rankName = rankName;
        output.texture = texture;
        output.specialEffects = specialEffects;
        
        return output;
    }
    
    public enum Suit
    {
        Heart = 1,
        Diamond = 2,
        Club = 4,
        Spade = 8
    }

    [Flags]
    public enum SpecialEffect
    {
        Double = 1,
        Blessed = 2,
        Shield = 4,
        Tutor = 8,
        Betray = 16,
        Lose = 32,
        SwapHand = 64,
    }
    
    public string rankName;
    public Texture texture;
    [NonSerialized] public Texture CardBack;
    [Range(0, 10)] public int rank;
    public SpecialEffect specialEffects;

    public bool HasSpecialEffect(SpecialEffect effect)
    {
        return specialEffects.HasFlag(effect);
    }
    
    public Color GetColour()
    {
        return Color.black;

        // return suit switch
        // {
        //     Suit.Heart => Color.red,
        //     Suit.Diamond => Color.yellow,
        //     Suit.Club => Color.blue,
        //     Suit.Spade => Color.black,
        //     _ => throw new ArgumentOutOfRangeException()
        // };
    }
}