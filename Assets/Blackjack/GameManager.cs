using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] public BlackjackManager blackjackManager;

    [SerializeField] private PlayerData playerData;

    private async void Awake()
    {
        if(Instance)
            Destroy(gameObject);
        else
            Instance = this;
        
        blackjackManager.Initialise();

        while (!destroyCancellationToken.IsCancellationRequested) {
            await blackjackManager.TakeTurn(destroyCancellationToken);
        }
    }
}
