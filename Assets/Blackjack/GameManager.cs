using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Combat.UI;
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
            catch (GameOverException e) {
                if (e.Winner == PlayerData.Character.Player) {
                    LogManager.Instance.Log(new LogData("Curse you - the luck of mortals...", "Lance"));
                }
                else {
                    LogManager.Instance.Log(new LogData("You seek to escape riding fledgling wings. Filth.", e.WinnerName));
                }
                
                CombatSceneLoader.Instance.UnloadCombat().Forget();
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

        await blackjackManager.UpdateWinners(token);
    }
}