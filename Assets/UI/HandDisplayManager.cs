using System.Collections.Generic;
using UnityEngine;

public class HandDisplayManager : MonoBehaviour
{
    [SerializeField] private CardObjectFactory cardObjectFactory;
    [SerializeField] private RectTransform handContainer;
    
    private Dictionary<CardInfo, CardObject> _cardObjects = new();
    
    private List<CardInfo> _currentCardInfos;

    public void DrawCard(CardInfo cardInfo)
    {
        var co = cardObjectFactory.Create(cardInfo);
        co.transform.SetParent(handContainer, false);
        _cardObjects.Add(cardInfo, co);
    }
}
