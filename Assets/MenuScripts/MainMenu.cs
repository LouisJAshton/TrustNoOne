using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Credits;
    [SerializeField] private GameObject Settings;

    [SerializeField] private SceneReference levelScene;
    [SerializeField] private AudioClip StartClip;

    private int startdelay;
    private void Start()
    {
        Menu.SetActive(true);
        Credits.SetActive(false);
    }
    public void StartGame()
    {
        if (startdelay == 0)
        {
            MainAudio.instance.PlaySFXClip(StartClip, transform, 1);
            startdelay = 1;
        }
    }
    public void OpenSettings()
    {
        if (startdelay == 0)
        {
            Menu.SetActive(false);
            Settings.SetActive(true);
        }
    }
    public void OpenCredits()
    {
        if (startdelay == 0) 
        { 
            Menu.SetActive(false);
            Credits.SetActive(true);
        }
    }
    public void CloseCredits()
    {
        if (startdelay == 0)
        {
            Menu.SetActive(true);
            Credits.SetActive(false);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void FixedUpdate()
    {
        if (startdelay >0)
        {
            startdelay++;
            if (startdelay > 170)
            {
                SceneManager.LoadScene(levelScene.BuildIndex);
            }
        }
    }
}
