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
        output.shaderInfo = shaderInfo;
        
        return output;
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
    public CardShaderInfo shaderInfo;

    public bool HasSpecialEffect(SpecialEffect effect)
    {
        return specialEffects.HasFlag(effect);
    }
    
    public Color GetColour()
    {
        return specialEffects == 0 ? Color.clear : Color.black;
    }

    [Serializable]
    public struct CardShaderInfo
    {
        [SerializeField] public Color shimmerColour;
        [SerializeField, Range(0, 1)] public float opacity;
    }
}