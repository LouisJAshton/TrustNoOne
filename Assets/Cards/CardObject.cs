using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class CardObject : MonoBehaviour
{
    [SerializeField] private BaseCardInfo baseCardInfo;

    [SerializeField] private RawImage image;
    [SerializeField] private TMP_Text[] ranks;

    [NonSerialized] public bool IsBeingDisposed = false;

    public void SetCardInfo(CardInfo cardInfo)
    {
        image.texture = cardInfo.texture;

        foreach (var rank in ranks) {
            rank.color = cardInfo.GetColour();
            rank.text = cardInfo.rankName;
        }
    }

    public async UniTask Dispose(Transform target, float animLength, CancellationToken token)
    {
        if (IsBeingDisposed)
            return;
        
        IsBeingDisposed = true;

        float time = Time.time;
        while (!token.IsCancellationRequested && Time.time - time < animLength) {
            var t = (Time.time - time) / animLength;
            
            transform.position = Vector3.Lerp(transform.position, target.position, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, t);
            await UniTask.Yield(cancellationToken: token);
        }
        
        Destroy(gameObject);

        return;
    }
}
