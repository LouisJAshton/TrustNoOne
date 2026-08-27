using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject Menu;

    [SerializeField] private SceneReference levelScene;
    [SerializeField] private AudioClip testclip;
    private void Start()
    {
        Menu.SetActive(true);
    }
    public void StartGame()
    {
        Debug.Log("UPDATE SCENE NAME TO LOAD");
        MainAudio.instance.PlaySFXClip(testclip, transform, 1);
        SceneManager.LoadScene(levelScene.BuildIndex);
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
