using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using LouisAshton.Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BlackjackPlayerButtonManager : Singleton<BlackjackPlayerButtonManager>
{
    [SerializeField] private Button standButton;
    [SerializeField] private Button hitButton;

    [SerializeField] private AudioClip HitSfx;
    [SerializeField] private AudioClip StandSfx;

    private void Awake()
    {
        standButton.gameObject.SetActive(false);
        hitButton.gameObject.SetActive(false);
    }

    public async UniTask<Response> GetPlayerInput(CancellationToken token)
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        
        EventSystem.current.SetSelectedGameObject(null); 
        
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

            if (standButton && standButton.IsPressed())
            {
                MainAudio.instance.PlaySFXClip(StandSfx, transform, 1);
                return Response.Stand;
            }
            if (hitButton && hitButton.IsPressed())
            { 
                MainAudio.instance.PlaySFXClip(StandSfx, transform, 1);
                return Response.Hit;
            }
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
