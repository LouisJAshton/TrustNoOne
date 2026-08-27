using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ITurnStrategy
{
    public UniTask TakeTurn(CancellationToken cancellationToken);
}

public class AITurnStrategy : ITurnStrategy
{
    private readonly PlayerData _self;

    public AITurnStrategy(PlayerData self)
    {
        _self = self;
    }
    
    public async UniTask TakeTurn(CancellationToken cancellationToken)
    {
        await UniTask.Delay(1000, cancellationToken: cancellationToken);

        if (BlackjackManager.CalculateScore(_self.Hand) < 18) {
            await GameManager.Instance.blackjackManager.Draw(_self, cancellationToken);
        }
        else {
            _self.IsStanding = true;
        }
    }
}

public class DealerTurnStrategy : ITurnStrategy
{
    private readonly PlayerData _self;

    public DealerTurnStrategy(PlayerData self)
    {
        _self = self;
    }

    public async UniTask TakeTurn(CancellationToken cancellationToken)
    {
        await UniTask.Delay(1000, cancellationToken: cancellationToken);

        if (BlackjackManager.CalculateScore(_self.Hand) < 18) {
            await GameManager.Instance.blackjackManager.Draw(_self, cancellationToken);
        }
        else {
            _self.IsStanding = true;
        }
    }
}

public class PlayerTurnStrategy : ITurnStrategy
{
    private readonly PlayerData _self;

    public PlayerTurnStrategy(PlayerData self)
    {
        _self = self;
    }
    
    public async UniTask TakeTurn(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                await GameManager.Instance.blackjackManager.Draw(_self, cancellationToken);
                break;
            }

            if (Input.GetKeyDown(KeyCode.Return)) {
                _self.IsStanding = true;
                break;
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                await GameManager.Instance.blackjackManager.Reshuffle(cancellationToken);
                break;
            }
            
            await UniTask.Yield();
        }
    }
}