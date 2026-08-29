using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardObjectFactory", menuName = "Scriptable Objects/CardObjectFactory")]
public class CardObjectFactory : ScriptableObject
{
    private static readonly int TimeOffset = Shader.PropertyToID("_TimeOffset");
    private static readonly int ShimmerColour = Shader.PropertyToID("_Shimmer_Colour");
    private static readonly int Opacity = Shader.PropertyToID("_Opacity");
    [SerializeField] private CardObject cardObjectPrefab;
    
    public CardObject Create(CardInfo cardInfo)
    {
        if (!cardObjectPrefab) {
            Debug.LogError("CardObjectFactory: cardObjectPrefab is null");
            return null;
        }
        
        var cardObject = Instantiate(cardObjectPrefab);

        if (cardObject.TryGetComponent<RawImage>(out var image)) {
            image.material = new Material(image.material);
            
            image.material.SetFloat(TimeOffset, Random.Range(0, 100000));
            image.material.SetColor(ShimmerColour, cardInfo.shaderInfo.shimmerColour);
            image.material.SetFloat(Opacity, cardInfo.shaderInfo.opacity);
        }
        
        cardObject.SetCardInfo(cardInfo);
        
        return cardObject;
    }
}
