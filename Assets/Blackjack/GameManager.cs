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
    
    [SerializeField] private RoundOverEvent roundOverEvent;
    
    private void Awake()
    {
        if(Instance && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        
        RunBattle(destroyCancellationToken).Forget();
    }

    private async UniTask RunBattle(CancellationToken token)
    {
        LogManager.Instance.Log(new LogData("You stride up to the table with infernal confidence...", "Narrator"));
        
        while (token.IsCancellationRequested == false) {
            await UniTask.WaitForSeconds(1, cancellationToken: token);
            
            try {
                await PlayRound(token);
                await UniTask.WaitForSeconds(1, cancellationToken: token);
            }
            catch (GameOverException e) {
                if (e.Winner == PlayerData.Character.Player) {
                    switch (e.Opponent) {
                        case DialogueHandler.CharacterName.Bar:
                            LogManager.Instance.Log(new LogData("You feel emboldened by this first victory", "Narrator"));
                            break;
                        case DialogueHandler.CharacterName.Agalia:
                            LogManager.Instance.Log(new LogData("Once was luck. Twice is a hot streak. You can't quit now...", "Narrator"));
                            break;
                        case DialogueHandler.CharacterName.Lance:
                            LogManager.Instance.Log(new LogData("...", "Narrator"));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    roundOverEvent.Trigger(new RoundOverEventData(e.Opponent, true));
                }
                else {
                    LogManager.Instance.Log(new LogData("Pathetic", e.WinnerName));
                    roundOverEvent.Trigger(new RoundOverEventData(e.Opponent, false));
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
        else {
            LogManager.Instance.Log(new LogData("21! The round ends at once.", "Dealer"));
        }

        await blackjackManager.UpdateWinners(token);
    }
}