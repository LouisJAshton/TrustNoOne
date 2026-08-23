using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] public BlackjackManager blackjackManager;
    
    private async void Awake()
    {
        if(Instance && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        while (destroyCancellationToken.IsCancellationRequested == false) {
            await PlayRound();
            await UniTask.WaitForSeconds(2, cancellationToken: destroyCancellationToken);
        }
    }

    private async Task PlayRound()
    {
        blackjackManager.Reshuffle();

        if (!blackjackManager.CheckForBlackjacks(out var blackjacks)) {
            while (!destroyCancellationToken.IsCancellationRequested) {
                try {
                    await blackjackManager.TakeTurn(destroyCancellationToken);
                }
                catch (RoundEndedException) {
                    break;
                }
            }
            
            await blackjackManager.Dealer(destroyCancellationToken);
        }
        
        var winners = await blackjackManager.CalculateWinner(destroyCancellationToken);

        foreach (var winner in winners) {
            print(winner.playerName + " wins!");
        }
    }
}
