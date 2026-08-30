using System;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField] private SceneReference mainMenu;
    
    private async void Start()
    {
        await UniTask.WaitForSeconds(5, cancellationToken: Application.exitCancellationToken);
        
        await SceneManager.LoadSceneAsync(mainMenu.BuildIndex, LoadSceneMode.Single);
    }
}
