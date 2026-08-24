using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class BlackjackManager
{
    public const int MAX = 21;

    public const int MAX_SCORE = 100;
    private int _currentScore = 0;
    
    private List<CardInfo> deck;

    [FormerlySerializedAs("player")] public PlayerData player1;
    public PlayerData player2;
    
    public PlayerData dealer;

    public UnityEvent<int> OnScoreChange;
    public UnityEvent<string[]> OnRoundWon;

    public void Initialise()
    {
        player1.turnStrategy = new PlayerTurnStrategy(player1);
        player2.turnStrategy = new AITurnStrategy(player2);
        dealer.turnStrategy = new DealerTurnStrategy(dealer);

        player1.ScoreUpdateStrategy = new PlayerScoreUpdateStrategy(this, player1);
        player2.ScoreUpdateStrategy = new EnemyScoreUpdateStrategy(this, player2);
        
        deck = new List<CardInfo>();
        var cards = Resources.LoadAll($"Cards", typeof(BaseCardInfo));
        foreach (var cardInfo in cards) {
            var card = cardInfo as BaseCardInfo;
            
            if (card) deck.Add(card.BaseInfo);
        }
        
        Draw(player1);
        Draw(player2);
        Draw(player1);
        Draw(player2);
        
        Draw(dealer);
        Draw(dealer);
    }

    public async UniTask TakeTurn(CancellationToken cancellationToken)
    {
        if (player1.IsStanding && player2.IsStanding) {
            throw new BothStandingException();
        }

        await TurnCycle(player1, cancellationToken);
        await TurnCycle(player2, cancellationToken);
    }

    private async UniTask TurnCycle(PlayerData player, CancellationToken token)
    {
        if (CalculateScore(player.Hand) > 21 || player.IsStanding)
            return;
        
        await player.turnStrategy.TakeTurn(token);
        if (CalculateScore(player.Hand) > 21) {
            player.IsStanding = true;
        }

        await UniTask.Yield();
    }
    
    public static int CalculateScore(List<CardInfo> cards)
    {
        var total = 0;
        var acesCount = 0;
        
        var totalHand = new List<CardInfo>();
        totalHand.AddRange(cards);

        foreach (var card in totalHand) {
            
            //Handle non-aces
            if (card.rank != 1) {
                total += card.rank;
            }
            //Handle aces
            else {
                acesCount++;
            }
        }

        for (var i = acesCount; i >= 1; i--) {
            var acesValue = i * 11;
            
            if (acesValue + total <= 21) {
                total += acesValue;
                break;
            }
            else {
                total += 1;
            }
        }
        
        return total;
    }

    public void Draw(PlayerData activePlayer)
    {
        if (deck == null || deck.Count == 0) {
            Debug.Log("Drawing from empty deck");
            return;
        }
        
        //TODO Replace with actual draw
        var card = deck[Random.Range(0, deck.Count)];
        deck.Remove(card);
        activePlayer.AddCards(card);
    }

    //TODO Use unitask for juice
    public void Reshuffle()
    {
        player1.Reset();
        player2.Reset();
        dealer.Reset();
        Initialise();
    }

    public void DebugDealerHand()
    {
        StringBuilder sb = new();
        foreach (var card in dealer.Hand) {
            sb.Append(card.rankName);
            sb.Append(" of ");
            sb.Append(card.suit);
            sb.Append("s | ");
        }
        
        Debug.Log(sb.ToString());
    }

    public async UniTask Dealer(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested) {
            await dealer.turnStrategy.TakeTurn(cancellationToken);
            if (dealer.IsStanding) {
                return;
            }
        }
    }

    public async UniTask<List<PlayerData>> CalculateWinner(CancellationToken cancellationToken)
    {
        await UniTask.Delay(1000, cancellationToken: cancellationToken);
        
        List<PlayerData> winners = new List<PlayerData>();
        List<PlayerData> players = new List<PlayerData>()
        {
            player1, player2, dealer
        };

        foreach (var player in players.ToList().Where(player => CalculateScore(player.Hand) > 21)) {
            players.Remove(player);
        }

        if (CheckForBlackjacks(players, out winners)) {
            return winners;
        }

        if (!players.Contains(dealer)) {
            return players;
        }

        foreach (var player in players) {
            if (CalculateScore(player.Hand) > CalculateScore(dealer.Hand)) {
                winners.Add(player);
            }
        }

        return winners;
    }

    public bool CheckForBlackjacks(out List<PlayerData> winners)
    {
        return CheckForBlackjacks(new List<PlayerData>() {player1, player2, dealer}, out winners);
    }
    
    public bool CheckForBlackjacks(List<PlayerData> players, out List<PlayerData> winners)
    {
        winners = new List<PlayerData>();
        
        foreach (var player in players) {
            if (player.Hand.Count != 2) {
                return false;
            }

            if (CalculateScore(player.Hand) == MAX) {
                winners.Add(player);
            }
        }
        
        return winners.Count > 0;
    }

    public async UniTask UpdateWinners(CancellationToken token)
    {
        var winners = await CalculateWinner(token);

        List<string> winnerStrings = new List<string>();
        foreach (var winner in winners) {
            winnerStrings.Add(winner.playerName);
        }
        
        OnRoundWon.Invoke(winnerStrings.ToArray());

        foreach (var winner in winners) {
            
        }
    }

    public UniTask ChangeScore(int delta, CancellationToken token)
    {
        _currentScore += delta;
        OnScoreChange.Invoke(_currentScore);
        return UniTask.CompletedTask;
    }
}