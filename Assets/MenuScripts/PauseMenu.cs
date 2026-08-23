using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject Pause;
    public GameObject PausedMenu;
    public GameObject SettingsMenu;
    public GameObject CreditsMenu;

    private void Awake()
    {
        Pause.SetActive(false);
        PausedMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
    }
    public void OpenPause()
    {
        Pause.SetActive(true);
        PausedMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        Time.timeScale = 0;
    }
    public void ClosePause()
    {
        Pause.SetActive(false);
        PausedMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenSettings()
    {
        PausedMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void CloseSettings() 
    {
        PausedMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void OpenCredits()
    {
        CreditsMenu.SetActive(true);
        PausedMenu.SetActive(false);
    }
}
