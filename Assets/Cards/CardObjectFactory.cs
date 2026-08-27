using UnityEngine;

[CreateAssetMenu(fileName = "CardObjectFactory", menuName = "Scriptable Objects/CardObjectFactory")]
public class CardObjectFactory : ScriptableObject
{
    [SerializeField] private CardObject cardObjectPrefab;
    
    public CardObject Create(CardInfo cardInfo)
    {
        if (!cardObjectPrefab) {
            Debug.LogError("CardObjectFactory: cardObjectPrefab is null");
            return null;
        }
        
        var cardObject = Instantiate(cardObjectPrefab);
        cardObject.SetCardInfo(cardInfo);
        
        return cardObject;
    }
}
