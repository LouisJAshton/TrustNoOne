using UnityEngine;

[CreateAssetMenu(fileName = "CardObjectFactory", menuName = "Scriptable Objects/CardObjectFactory")]
public class CardObjectFactory : BaseCardObjectFactory
{
    [SerializeField] private CardObject cardObjectPrefab;
    
    public override GameObject Create(CardInfo cardInfo)
    {
        if (!cardObjectPrefab) {
            Debug.LogError("CardObjectFactory: cardObjectPrefab is null");
            return null;
        }
        
        var cardObject = Instantiate(cardObjectPrefab);
        cardObject.SetCardInfo(cardInfo);
        
        return cardObject.gameObject;
    }
}

public abstract class BaseCardObjectFactory : ScriptableObject
{
    public abstract GameObject Create(CardInfo cardInfo);
}
