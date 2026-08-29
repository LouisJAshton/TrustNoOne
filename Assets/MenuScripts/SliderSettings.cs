using UnityEngine;

public class SliderSettings : MonoBehaviour
{
    void Awake()
    {
        //Slider        
        if (!PlayerPrefs.HasKey("MusicSoundVol"))
        {
            PlayerPrefs.SetFloat("MusicSoundVol", 1);
        }
        if (!PlayerPrefs.HasKey("SFXSoundVol"))
        {
            PlayerPrefs.SetFloat("SFXSoundVol", 1);
        }
        if (!PlayerPrefs.HasKey("MasterSoundVol"))
        {
            PlayerPrefs.SetFloat("VoiceSoundVol", 1);
        }
        if (!PlayerPrefs.HasKey("CameraSensitivity"))
        {
            PlayerPrefs.SetFloat("CameraSensitivity", 1);
        }
    }


}
