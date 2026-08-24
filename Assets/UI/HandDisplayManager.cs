using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandDisplayManager : MonoBehaviour
{
    //TODO use shared serialised reference
    [SerializeField] private string playerName;
    
    [SerializeField] private BaseCardObjectFactory cardObjectFactory;
    [SerializeField] private RectTransform handContainer;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image standingImage;
    [SerializeField] private ParticleSystem winParticles;
    
    private Dictionary<CardInfo, GameObject> _cardObjects = new();

    private void Start()
    {
        GameManager.Instance.blackjackManager.OnRoundWon.AddListener(PlayParticles);
    }

    private void PlayParticles(params string[] player)
    {
        if (player.Contains(playerName)) {
            winParticles.Play();
        }
    }

    public void DrawCard(CardInfo cardInfo)
    {
        var co = cardObjectFactory.Create(cardInfo);
        co.transform.SetParent(handContainer, false);
        _cardObjects.Add(cardInfo, (GameObject)co);
    }

    public void UpdateStanding(bool isStanding)
    {
        // nameText.color = !isStanding ? Color.green : Color.red;
        standingImage.enabled = isStanding;
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

        var score = BlackjackManager.CalculateScore(cardInfos);
        scoreText.text = score > 21 ? "BUST" : score.ToString();

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
