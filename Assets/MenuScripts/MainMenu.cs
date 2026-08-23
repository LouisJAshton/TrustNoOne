using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject Menu;

    [SerializeField] private AudioClip testclip;
    private void Start()
    {
        Menu.SetActive(true);
    }
    public void StartGame()
    {
        Debug.Log("UPDATE SCENE NAME TO LOAD");
        MainAudio.instance.PlaySFXClip(testclip, transform, 1);
        //SceneManager.LoadScene("Office");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
