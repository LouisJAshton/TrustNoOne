using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandDisplayManager : MonoBehaviour
{
    //TODO use shared serialised reference
    [SerializeField] private PlayerData.Character character;
    
    [SerializeField] private CardObjectFactory cardObjectFactory;
    [SerializeField] private RectTransform handContainer;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image standingImage;
    [SerializeField] private Image shieldedImage;
    [SerializeField] private ParticleSystem winParticles;

    [SerializeField] private Transform binTransform;
    
    [SerializeField] private CombatContext combatContext;
    
    private Dictionary<CardInfo, CardObject> _cardObjects = new();

    private void Start()
    {
        GameManager.Instance.blackjackManager.OnRoundWon.AddListener(PlayParticles);
        
        //Messy way to only get opponent to change name
        if (combatContext)
            nameText.text = combatContext.EnemyData.enemyName;
    }

    private void PlayParticles(params PlayerData.Character[] player)
    {
        if (player.Contains(character)) {
            winParticles.Play();
        }
    }

    public void DrawCard(CardInfo cardInfo)
    {
        var co = cardObjectFactory.Create(cardInfo);
        co.transform.SetParent(handContainer, false);
        
        //TODO Maybe set draw from animation elsewhere?
        co.transform.position = CardGameBaseObject.Instance.transform.position;
        _cardObjects.Add(cardInfo, co);
    }

    public void UpdateStanding(bool isStanding)
    {
        // nameText.color = !isStanding ? Color.green : Color.red;
        standingImage.enabled = isStanding;
    }

    public void UpdateShielded(bool isShielded)
    {
        shieldedImage.enabled = isShielded;
    }

    // public void Clear()
    // {
    //     foreach (var cardInfo in _cardObjects.ToList()) {
    //         Destroy(cardInfo.Value.gameObject);
    //     }
    //     
    //     _cardObjects.Clear();
    // }

    public async UniTask UpdateHand(List<CardInfo> cardInfos)
    {
        var toAdd = new List<CardInfo>();
        var toRemove = new List<CardInfo>();

        var score = BlackjackManager.CalculateScore(cardInfos);
        scoreText.text = score > 21 ? score.ToString() +  " BUST" : score.ToString();

        foreach (var cardInfo in cardInfos) {
            if(!_cardObjects.ContainsKey(cardInfo))
                toAdd.Add(cardInfo);
        }

        foreach (var kvp in _cardObjects) {
            if(!cardInfos.Contains(kvp.Key))
                toRemove.Add(kvp.Key);
        }

        var disposedCardOps = new List<UniTask>();

        foreach (var card in toRemove) {
            await UniTask.WaitForSeconds(0.1f, cancellationToken: destroyCancellationToken);
            disposedCardOps.Add(_cardObjects[card].Dispose(binTransform, 1f, destroyCancellationToken));
        }
        
        await UniTask.WhenAll(disposedCardOps);
        
        foreach (var card in toRemove) {
            _cardObjects.Remove(card);
        }

        foreach (var card in toAdd) {
            DrawCard(card);
        }
    }
}
