using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ITurnStrategy
{
    public UniTask<bool> TakeTurn(CancellationToken cancellationToken);
}

public class AITurnStrategy : ITurnStrategy
{
    private readonly PlayerData _self;

    public AITurnStrategy(PlayerData self)
    {
        _self = self;
    }
    
    public async UniTask<bool> TakeTurn(CancellationToken cancellationToken)
    {
        if (_self.IsStanding)
            return true;
        
        await UniTask.Delay(1000, cancellationToken: cancellationToken);

        if (BlackjackManager.CalculateScore(_self.Hand) < 18) {
            GameManager.Instance.blackjackManager.Draw(_self);
        }
        else {
            _self.IsStanding = true;
        }
        
        return _self.IsStanding;
    }
}

public class DealerTurnStrategy : ITurnStrategy
{
    private readonly PlayerData _self;

    public DealerTurnStrategy(PlayerData self)
    {
        _self = self;
    }

    public async UniTask<bool> TakeTurn(CancellationToken cancellationToken)
    {
        if (_self.IsStanding)
            return true;
        
        await UniTask.Delay(1000, cancellationToken: cancellationToken);

        if (BlackjackManager.CalculateScore(_self.Hand) < 18) {
            GameManager.Instance.blackjackManager.Draw(_self);
        }
        else {
            _self.IsStanding = true;
        }
        
        return _self.IsStanding;
    }
}

public class PlayerTurnStrategy : ITurnStrategy
{
    private readonly PlayerData _self;

    public PlayerTurnStrategy(PlayerData self)
    {
        _self = self;
    }
    
    public async UniTask<bool> TakeTurn(CancellationToken cancellationToken)
    {
        if (BlackjackManager.CalculateScore(_self.Hand) > 21)
            return true;

        if (_self.IsStanding)
            return true;
        
        while (!cancellationToken.IsCancellationRequested) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameManager.Instance.blackjackManager.Draw(_self);
                break;
            }

            if (Input.GetKeyDown(KeyCode.Return)) {
                _self.IsStanding = true;
                break;
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                GameManager.Instance.blackjackManager.Reshuffle();
                break;
            }
            
            await UniTask.Yield();
        }

        return _self.IsStanding;
    }
}