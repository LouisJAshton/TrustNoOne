using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BlackjackManager blackjackManager;

    [SerializeField] private PlayerData playerData;

    private void Awake()
    {
        blackjackManager.Initialise();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            blackjackManager.Draw();
            blackjackManager.DebugDealerHand();
        }
        
        if (Input.GetKeyDown(KeyCode.Return)) {
            var score = blackjackManager.CalculateScore(blackjackManager.hand);
            Debug.Log(score);
            if (score > 21) {
                Debug.Log("Bust!");
            }
        }
    }
}
