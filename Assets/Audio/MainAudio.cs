using UnityEngine;

public class MainAudio : MonoBehaviour
{
    public static MainAudio instance;//can be called anywhere

    public AudioSource SFXObj;
    public AudioSource MusicObj;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void PlaySFXClip(AudioClip Clip, Transform spawnpoint, float volume)
    {
        AudioSource audioSource = Instantiate(SFXObj, spawnpoint.position, Quaternion.identity);

        audioSource.clip = Clip;

        audioSource.volume = volume;

        audioSource.Play();

        float length = audioSource.clip.length;

        Destroy(audioSource.gameObject, length);
    }

    public void PlayVoiceClip(AudioClip Clip, Transform spawnpoint, float volume)
    {
        AudioSource audioSource = Instantiate(MusicObj, spawnpoint.position, Quaternion.identity);

        audioSource.clip = Clip;

        audioSource.volume = volume;

        audioSource.Play();

        float length = audioSource.clip.length;

        Destroy(audioSource.gameObject, length);
    }

}
