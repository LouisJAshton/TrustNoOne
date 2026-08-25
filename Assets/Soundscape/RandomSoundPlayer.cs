using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField] private List<SoundInfo> sounds;
    [SerializeField] private float avgDelaySeconds = 10;
    [SerializeField] private float rangeDelaySeconds = 4;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Cycle(destroyCancellationToken).Forget();
    }

    private async UniTask Cycle(CancellationToken token)
    {
        await UniTask.WaitForSeconds(Random.Range(avgDelaySeconds - rangeDelaySeconds / 2, avgDelaySeconds + rangeDelaySeconds / 2), cancellationToken: token);
        PlaySound(sounds[Random.Range(0, sounds.Count)]);

        if (token.IsCancellationRequested)
            return;

        Cycle(token).Forget();
    }
    
    private void PlaySound(SoundInfo soundInfo)
    {
        _audioSource.Stop();
        _audioSource.clip = soundInfo.GetClip();

        if (!_audioSource.clip)
            return;
        
        _audioSource.volume = 1 * Random.Range(soundInfo.minVolumeScalar, soundInfo.maxVolumeScalar);
        _audioSource.pitch = 1 * Random.Range(soundInfo.minPitchScalar, soundInfo.maxPitchScalar);
        _audioSource.Play();
    }
}

[Serializable]
public struct SoundInfo
{
    public string name;
    public List<AudioClip> clipPool;
    public float minVolumeScalar;
    public float maxVolumeScalar;
    public float minPitchScalar;
    public float maxPitchScalar;

    public AudioClip GetClip()
    {
        return clipPool.Count == 0 ? null : clipPool[Random.Range(0, clipPool.Count)];
    }
}
