using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ITurnStrategy
{
    public UniTask TakeTurn(PlayerData self, CancellationToken cancellationToken);
}

public class DealerTurnStrategy : ITurnStrategy
{
    public async UniTask TakeTurn(PlayerData self, CancellationToken cancellationToken)
    {
        await UniTask.Delay(1000, cancellationToken: cancellationToken);
        
        if (BlackjackManager.CalculateScore(self.Hand) < 18) {
            GameManager.Instance.blackjackManager.Draw(self);
        }
    }
}

public class PlayerTurnStrategy : ITurnStrategy
{
    public async UniTask TakeTurn(PlayerData self, CancellationToken cancellationToken)
    {
        if (BlackjackManager.CalculateScore(self.Hand) > 21)
            return;
        
        while (!cancellationToken.IsCancellationRequested) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameManager.Instance.blackjackManager.Draw(self);
                break;
            }

            if (Input.GetKeyDown(KeyCode.Return)) {
                break;
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                GameManager.Instance.blackjackManager.Reshuffle();
                break;
            }
            
            await UniTask.Yield();
        }
    }
}