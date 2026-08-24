using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

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
            try {
                await PlayRound(destroyCancellationToken);
                await UniTask.WaitForSeconds(2, cancellationToken: destroyCancellationToken);
            }
            catch (GameOverException) {
                print("Game Over");
                break;
            }
        }
    }

    private async UniTask PlayRound(CancellationToken token)
    {
        await blackjackManager.Reshuffle(token);

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
        
        await blackjackManager.UpdateWinners(destroyCancellationToken);
    }
}