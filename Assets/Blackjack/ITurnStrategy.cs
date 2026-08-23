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
        
        if (BlackjackManager.CalculateScore(self.Hand) <= 18) {
            GameManager.Instance.blackjackManager.Draw(self);
        }
    }
}

public class PlayerTurnStrategy : ITurnStrategy
{
    public async UniTask TakeTurn(PlayerData self, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameManager.Instance.blackjackManager.Draw(self);
                break;
            }
        
            if (Input.GetKeyDown(KeyCode.Return)) {
                var score = BlackjackManager.CalculateScore(GameManager.Instance.blackjackManager.dealer.Hand);
                Debug.Log(score);
                if (score > 21) {
                    Debug.Log("Bust!");
                }

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