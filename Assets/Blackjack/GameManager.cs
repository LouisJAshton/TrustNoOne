using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BlackjackManager blackjackManager;

    [SerializeField] private PlayerData playerData;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            blackjackManager.Draw();
            blackjackManager.DebugDealerHand();
        }
        
        if (Input.GetKeyDown(KeyCode.Return)) {
            Debug.Log(blackjackManager.CalculateScore(playerData));
        }
    }
}
