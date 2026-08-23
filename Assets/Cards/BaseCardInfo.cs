using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class BaseCardInfo : ScriptableObject
{
    public CardInfo baseCardInfo;
}

[Serializable]
public struct CardInfo
{
    //[Flags]
    public enum Suit
    {
        Heart = 1,
        Diamond = 2,
        Club = 4,
        Spade = 8
    }

    public string rankName;
    public Texture texture;
    [Range(1, 10)] public int rank;
    public Suit suit;

    public Color GetColour()
    {
        return suit switch
        {
            Suit.Heart => Color.red,
            Suit.Diamond => Color.yellow,
            Suit.Club => Color.blue,
            Suit.Spade => Color.black,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}