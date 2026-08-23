using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandDisplayManager : MonoBehaviour
{
    [SerializeField] private CardObjectFactory cardObjectFactory;
    [SerializeField] private RectTransform handContainer;
    
    private Dictionary<CardInfo, CardObject> _cardObjects = new();
    
    public void DrawCard(CardInfo cardInfo)
    {
        var co = cardObjectFactory.Create(cardInfo);
        co.transform.SetParent(handContainer, false);
        _cardObjects.Add(cardInfo, co);
    }

    public void Clear()
    {
        foreach (var cardInfo in _cardObjects.ToList()) {
            Destroy(cardInfo.Value.gameObject);
        }
        
        _cardObjects.Clear();
    }

    //TODO Integrate with unitask for juice
    public void UpdateHand(List<CardInfo> cardInfos)
    {
        var toAdd = new List<CardInfo>();
        var toRemove = new List<CardInfo>();

        foreach (var cardInfo in cardInfos) {
            if(!_cardObjects.ContainsKey(cardInfo))
                toAdd.Add(cardInfo);
        }

        foreach (var kvp in _cardObjects) {
            if(!cardInfos.Contains(kvp.Key))
                toRemove.Add(kvp.Key);
        }

        foreach (var card in toAdd) {
            DrawCard(card);
        }

        foreach (var card in toRemove) {
            Destroy(_cardObjects[card].gameObject);
            _cardObjects.Remove(card);
        }
    }
}
