using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ITutorStrategy
{
    public UniTask Tutor(CancellationToken token);
}

public class PlayerTutorStrategy : ITutorStrategy
{
    private readonly PlayerData _playerData;
    private readonly BlackjackManager _blackjackManager;
    
    public PlayerTutorStrategy(PlayerData playerData, BlackjackManager blackjackManager)
    {
        _playerData = playerData;
        _blackjackManager = blackjackManager;
    }
    
    public UniTask Tutor(CancellationToken token)
    {
        throw new System.NotImplementedException();
    }
}

public class AITutorStrategy : ITutorStrategy
{
    private readonly PlayerData _playerData;
    private readonly BlackjackManager _blackjackManager;
    
    public AITutorStrategy(PlayerData playerData, BlackjackManager blackjackManager)
    {
        _playerData = playerData;
        _blackjackManager = blackjackManager;
    }
    
    public async UniTask Tutor(CancellationToken token)
    {
        Debug.Log("Tutoring...");

        var deck = _blackjackManager.deck;
        var currentScore = BlackjackManager.CalculateScore(_playerData);
        var delta = BlackjackManager.MAX - currentScore;
        
        delta = Mathf.Min(delta, 11);

        var card = SearchCardOfRank(delta);
        
        await UniTask.WaitForSeconds(1, cancellationToken: token);

        if (card != null)
            _playerData.AddCards(card);
        
        return;


        CardInfo SearchCardOfRank(int delta1)
        {
            for (int i = delta1; i >= 0; i--) {
                foreach (var deckCard in deck) {
                    if(deckCard.specialEffects == 0 && (deckCard.rank == i || (deckCard.rank == 1 && delta == 11)))
                        return deckCard;            
                }
            }

            return null;
        }
    }
}