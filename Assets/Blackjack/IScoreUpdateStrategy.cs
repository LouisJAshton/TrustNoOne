using System.Threading;
using Cysharp.Threading.Tasks;

//TODO Refine how scoring works

public interface IScoreUpdateStrategy
{
    public UniTask UpdateScore(CancellationToken token);
}

public class PlayerScoreUpdateStrategy : IScoreUpdateStrategy
{
    private readonly PlayerData _playerData;
    private readonly BlackjackManager _blackjackManager;
    
    public PlayerScoreUpdateStrategy(BlackjackManager blackjackManager, PlayerData playerData)
    {
        _blackjackManager = blackjackManager;
        _playerData = playerData;
    }
    
    public async UniTask UpdateScore(CancellationToken token)
    {
        int score = BlackjackManager.CalculateScore(_playerData.Hand);
        if (score > BlackjackManager.MAX) {
            return;
        }
        
        score -= BlackjackManager.CalculateScore(_blackjackManager.dealer.Hand) % BlackjackManager.MAX;

        await _blackjackManager.ChangeScore(score, token);
    }
}

public class EnemyScoreUpdateStrategy : IScoreUpdateStrategy
{
    private readonly PlayerData _playerData;
    private readonly BlackjackManager _blackjackManager;
    
    public EnemyScoreUpdateStrategy(BlackjackManager blackjackManager, PlayerData playerData)
    {
        _blackjackManager = blackjackManager;
        _playerData = playerData;
    }
    
    public async UniTask UpdateScore(CancellationToken token)
    {
        int score = BlackjackManager.CalculateScore(_playerData.Hand);
        if (score > BlackjackManager.MAX) {
            return;
        }
        
        score -= BlackjackManager.CalculateScore(_blackjackManager.dealer.Hand);// % BlackjackManager.MAX;

        await _blackjackManager.ChangeScore(-score, token);
    }
}