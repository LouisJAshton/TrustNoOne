using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] public BlackjackManager blackjackManager;
    
    private async void Awake()
    {
        if(Instance)
            Destroy(gameObject);
        else
            Instance = this;
        
        blackjackManager.Initialise();
        
        while (!destroyCancellationToken.IsCancellationRequested) {
            try {
                await blackjackManager.TakeTurn(destroyCancellationToken);
            }
            catch (RoundEndedException) {
                break;
            }
        }
        
        await blackjackManager.Dealer(destroyCancellationToken);
    }
}
