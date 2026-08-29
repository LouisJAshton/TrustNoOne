using UnityEngine;

public class MainAudio : MonoBehaviour
{
    public static MainAudio instance;//can be called anywhere

    public AudioSource SFXObj;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
}
