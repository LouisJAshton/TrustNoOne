using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class BaseCardInfo : ScriptableObject
{
    [SerializeField] private CardInfo baseCardInfo;
}

[Serializable]
public struct CardInfo
{
    [Flags]
    public enum Suit
    {
        Heart = 1,
        Diamond = 2,
        Club = 4,
        Spade = 8
    }
    
    public Texture2D texture;
    public int rank;
    public Suit suit;
}