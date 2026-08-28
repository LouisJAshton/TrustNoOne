using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using LouisAshton.Singletons;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BlackjackPlayerButtonManager : Singleton<BlackjackPlayerButtonManager>
{
    [SerializeField] private Button standButton;
    [SerializeField] private Button hitButton;

    private void Awake()
    {
        standButton.gameObject.SetActive(false);
        hitButton.gameObject.SetActive(false);
    }

    public async UniTask<Response> GetPlayerInput(CancellationToken token)
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        
        standButton.gameObject.SetActive(true);
        hitButton.gameObject.SetActive(true);
        
        Response r = await CheckForClick();
        
        standButton.gameObject.SetActive(false);
        hitButton.gameObject.SetActive(false);
        
        return r;
    }

    private async UniTask<Response> CheckForClick()
    {
        while (!destroyCancellationToken.IsCancellationRequested) {
            
            if(standButton && standButton.IsPressed())
                return Response.Stand;
            if(hitButton && hitButton.IsPressed())
                return Response.Hit;
            
            await UniTask.Yield(cancellationToken: destroyCancellationToken);
        }

        return Response.Stand;
    }

    public enum Response
    {
        Stand,
        Hit
    }
}
