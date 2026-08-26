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
    
    private void Awake()
    {
        RunBattle(destroyCancellationToken).Forget();
    }

    private async UniTask RunBattle(CancellationToken token)
    {
        if(Instance && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        while (token.IsCancellationRequested == false) {
            await UniTask.WaitForSeconds(1, cancellationToken: token);
            
            try {
                await PlayRound(token);
                await UniTask.WaitForSeconds(1, cancellationToken: token);
            }
            catch (GameOverException) {
                print("Game Over");
                break;
            }
            catch (OperationCanceledException) {
                
            }
        }
    }

    private async UniTask PlayRound(CancellationToken token)
    {
        await blackjackManager.Reshuffle(token);

        if (!blackjackManager.CheckForBlackjacks(out var blackjacks)) {
            while (!token.IsCancellationRequested) {
                try {
                    await blackjackManager.TakeTurn(token);
                }
                catch (RoundEndedException) {
                    break;
                }
            }
            
            await blackjackManager.Dealer(token);
        }

        try {
            await blackjackManager.UpdateWinners(token);
        }
        catch (GameOverException goe) {
            print($"{goe.WinnerName} wins!");
            CombatSceneLoader.Instance.UnloadCombat().Forget();
        }
    }
}