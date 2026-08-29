using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private InputAction PauseP;
    private InputAction PauseUI;

    public GameObject Pause;
    public GameObject PausedMenu;
    public GameObject SettingsMenu;
    public GameObject CreditsMenu;

    private bool paused = false;

    private void Awake()
    {
        PauseP = InputSystem.actions.FindAction("PauseP");
        PauseUI = InputSystem.actions.FindAction("PauseUI");

        Pause.SetActive(false);
        PausedMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
    }
    public void OpenPause()
    {
        gameObject.GetComponent<PlayerMovement>().ispaused = true;
        paused = true;
        Pause.SetActive(true);
        PausedMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ClosePause()
    {
        gameObject.GetComponent<PlayerMovement>().ispaused = false;
        if (!gameObject.GetComponent<PlayerInteract>().istalking)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        paused = false;
        Pause.SetActive(false);
        PausedMenu.SetActive(false);
        Time.timeScale = 1;
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
    public void CloseCredits()
    {
        CreditsMenu.SetActive(false);
        PausedMenu.SetActive(true);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    private void Update()
    {
        if (PauseP.WasPerformedThisFrame() || PauseUI.WasPerformedThisFrame())
        {
            if (paused)
            {
                ClosePause();
            }
            else
            {
                OpenPause();
            }
        }
    }
}
