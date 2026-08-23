using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMixer : MonoBehaviour
{
    [SerializeField] public AudioMixer mixer;
    public Slider Master;
    public Slider Music;
    public Slider SFX;

    public float MasterVol;
    public float MusVol;
    public float SFXVol;

    private void Awake()
    {
        MasterVol = PlayerPrefs.GetFloat("MasterSoundVol", 1);
        MusVol = PlayerPrefs.GetFloat("MusicSoundVol", 1);
        SFXVol = PlayerPrefs.GetFloat("SFXSoundVol", 1);
        Debug.Log("loaded as " + MasterVol);
        
        Master.value = MasterVol;
        Music.value = MusVol;
        SFX.value = SFXVol;
    }
    private void Start()
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(MasterVol) * 20);
        mixer.SetFloat("MusicVol", Mathf.Log10(MusVol) * 20);
        mixer.SetFloat("SFXVol", Mathf.Log10(SFXVol) * 20);
    }

    public void UpdateMaster(float value)
    {
        PlayerPrefs.SetFloat("MasterSoundVol", value);
        Debug.Log("Saved as: " + value);
        mixer.SetFloat("MasterVol", Mathf.Log10(value) * 20);
    }
    public void UpdateMusic(float value)
    {
        PlayerPrefs.SetFloat("MusicSoundVol", value);
        mixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
    }
    public void UpdateSFX(float value)
    {
        PlayerPrefs.SetFloat("SFXSoundVol", value);
        mixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
    }
}
